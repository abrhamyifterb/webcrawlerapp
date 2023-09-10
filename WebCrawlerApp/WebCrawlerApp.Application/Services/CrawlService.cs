using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using WebCrawlerApp.Application.Interfaces;
using WebCrawlerApp.Application.Dtos;
using RobotsTxt;
using WebCrawlerApp.Core.Interfaces;
using AutoMapper;

namespace WebCrawlerApp.Application.Services
{
    public class CrawlService : ICrawlService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CrawlService> _logger;
        private readonly ConcurrentQueue<QueueItem> _crawlQueue = new ConcurrentQueue<QueueItem>();
        private readonly ConcurrentDictionary<string, bool> _crawledSites = new ConcurrentDictionary<string, bool>();
        private readonly int maxRetryCount = 3;

        public CrawlService(HttpClient httpClient, ILogger<CrawlService> logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        private class QueueItem
        {
            public string Url { get; set; }
            public string BoundaryRegExp { get; set; }
            public int RetryCount { get; set; }
        }

        async Task<ResponseDTO<List<CrawledDataDTO>>> ICrawlService.CrawlWebsite(string url, string boundaryRegExp)
        {
            var response = new ResponseDTO<List<CrawledDataDTO>>();
            try {
                List<CrawledDataDTO> crawledPages = new List<CrawledDataDTO>();

                List<Task> tasks = new List<Task>();

                _crawlQueue.Enqueue(new QueueItem { Url = url, BoundaryRegExp = boundaryRegExp });

                for (int i = 0; i < 5; i++)  
                {
                    tasks.Add(Task.Run(async () =>
                    {
                        await Worker(crawledPages);
                    }));
                }

                await Task.WhenAll(tasks);
                
                Reset();
                response.Data = crawledPages;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Failed to crawl website");
                response.ErrorMessage = "An error occurred while crawling the website.";
            }
            return response;
        }

        private async Task Worker(List<CrawledDataDTO> crawledPages)
        {
            while (_crawlQueue.TryDequeue(out var item))
            {
                var normalizedUrl = item.Url;
                if (string.IsNullOrEmpty(normalizedUrl))
                {
                    _logger.LogWarning($"URL could not be normalized and will be skipped: {item.Url}");
                    continue;
                }
                if (!_crawledSites.TryAdd(normalizedUrl, true))
                {
                    _logger.LogInformation($"Already crawled: {item.Url}. Skipping.");
                    continue; 
                }
                if (item.RetryCount > maxRetryCount)
                {
                    _logger.LogWarning($"Max retry attempts reached for {item.Url}");
                    continue;
                }

                var crawledData = await CrawlSinglePage(item.Url, item.BoundaryRegExp);
                
                if (crawledData != null)
                {

                    lock (crawledPages)
                    {
                        crawledPages.Add(crawledData);
                    }

                    foreach (var link in crawledData.Links)
                    {
                        _crawlQueue.Enqueue(new QueueItem { Url = link, BoundaryRegExp = item.BoundaryRegExp });
                    }
                }
                else
                {
                    item.RetryCount++;
                    _crawlQueue.Enqueue(item);
                    _logger.LogWarning($"Retrying {item.Url}. Attempt #{item.RetryCount}");
                }
            }
        }

        private async Task<CrawledDataDTO> CrawlSinglePage(string url, string boundaryRegExp)
        {
            try
            {
                if (!await CanCrawl(url))
                {
                    _logger.LogWarning($"Crawling is disallowed by robots.txt for {url}");
                    return null;
                }

                //url = NormalizeUrl(url);

                var httpResponse = await _httpClient.GetAsync(url);

                if (!httpResponse.IsSuccessStatusCode)
                {
                    _logger.LogWarning($"Failed to fetch {url}. Status Code: {httpResponse.StatusCode}");
                    return null;
                }

                var content = await httpResponse.Content.ReadAsStringAsync();
                
                var parsedProcessedDocument = HtmlDocumentParser(content, url, boundaryRegExp);

                return parsedProcessedDocument.Result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while crawling {url}");
                return null;
            }
        }
        private Task<CrawledDataDTO> HtmlDocumentParser(string content, string url, string boundaryRegExp){
            var doc = new HtmlDocument();
            doc.LoadHtml(content);

            var titleNode = doc.DocumentNode.SelectSingleNode("//head/title");
            var title = titleNode != null ? titleNode.InnerText : "No title found";

            bool boundaryRegExpChecker = Regex.IsMatch(url, boundaryRegExp);
            var links = new List<string>();

            if(boundaryRegExpChecker) {
                var linkNodes = doc.DocumentNode.SelectNodes("//a[@href]");
                string baseUrl = url;
                links = linkNodes != null
                            ? linkNodes
                                .Select(n => n.GetAttributeValue("href", ""))
                                .Select(link => NormalizeUrl(link, baseUrl))
                                .Where(link => link != null && !link.StartsWith("mailto:"))
                                .Distinct()
                                .Where(link => link != baseUrl)
                                .ToList()
                            : new List<string>();

            }

            var parsedProcessed = new CrawledDataDTO
            {
                Id = Guid.NewGuid(),
                Url = url,
                Title = title,
                IsRestricted = !boundaryRegExpChecker,
                Links = links
            };

            return Task.FromResult(parsedProcessed);
        }
        private async Task<bool> CanCrawl(string url)
        {
            try
            {
                //return true;
                var robotsTxtUrl = new Uri(new Uri(url), "/robots.txt");
                var httpResponse = await _httpClient.GetAsync(robotsTxtUrl);
                if (!httpResponse.IsSuccessStatusCode)
                {
                    return true; 
                }

                var robotsTxtContent = await httpResponse.Content.ReadAsStringAsync();
                var robotsTxt = Robots.Load(robotsTxtContent);

                return robotsTxt.IsPathAllowed("*", url);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"An error occurred while checking robots.txt for {url}. Assuming crawling is allowed.");
                return true;
            }
        }

        private string NormalizeUrl(string url, string baseUrl)
        {
            try
            {
                if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
                {
                    url = new Uri(new Uri(baseUrl), url).ToString();
                }
                var uri = new Uri(url);
                
                var domainAndScheme = $"{uri.Scheme}://{uri.Host}".ToLower();

                var normalizedUrl = domainAndScheme + uri.AbsolutePath;

                normalizedUrl = normalizedUrl.TrimEnd('/');

                var queryIndex = normalizedUrl.IndexOf('?');
                if (queryIndex != -1)
                {
                    normalizedUrl = normalizedUrl.Substring(0, queryIndex);
                }

                return normalizedUrl;
            }
            catch (UriFormatException ex)
            {
                _logger.LogError(ex, $"Invalid URL format: {url}");
                return null;  // Return null or handle this error in a way that makes sense for your application.
            }
        }

        private void Reset()
        {
            _crawlQueue.Clear();
            _crawledSites.Clear();
        }
    }
}
