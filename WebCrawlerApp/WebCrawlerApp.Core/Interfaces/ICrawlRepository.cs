using WebCrawlerApp.Core.Entities;
using System;

namespace WebCrawlerApp.Core.Interfaces 
{
    public interface ICrawlRepository
    {
        Task<int> Add(CrawledData crawledData);
        Task<CrawledData> CrawlWebsite(string url, string boundaryRegExp);
        Task<int> AddRange(IEnumerable<CrawledData> crawledData);

    }
}