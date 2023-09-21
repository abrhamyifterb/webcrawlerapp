using Newtonsoft.Json;
using WebCrawlerApp.Core.Entities;
using WebCrawlerApp.Infrastructure.Data;
using WebCrawlerApp.API.GraphQL.Schemas;
using System.Linq;

namespace WebCrawlerApp.API.GraphQL
{
    public class Query
    {

    /// <summary>
    /// Retrievs all the websites from the database
    /// </summary>
    /// <param name="dbContext"></param>
    /// <returns></returns>
    public List<WebPage> GetWebsites([Service] AppDbContext dbContext)
    {
        return dbContext.Websites.Select(w => new WebPage
        {
            Identifier = w.Id,
            Label = w.Label,
            Url = w.Url,
            Regexp = w.BoundaryRegExp,
            Tags = w.Tags.ToList(),
            Active = w.IsActive,
        }).ToList();
    }

    /// <summary>
    /// Gets Nodes (CrawledData) from the database according to the passed webpageids 
    /// </summary>
    /// <param name="webPages"></param>
    /// <param name="dbContext"></param>
    /// <returns></returns>
    public List<Node> GetNodes(List<Guid> webPages, [Service] AppDbContext dbContext)
    {
        var sites = dbContext.Websites.Where(w => webPages.Contains(w.Id)).ToList();
        
        var nodes = new List<Node>();
        foreach(var site in sites)
        {
            var crawledData = JsonConvert.DeserializeObject<List<CrawledData>>(site.CrawledData);
            foreach(var data in crawledData)
            {
                var node = new Node
                {
                    Title = data.Title ?? "",
                    Url = data.Url,
                    CrawlTime = data.CrawlTime,
                    Links = data.Links.Select(link => new Node { Url = link }).ToList(),
                    Owner = new WebPage 
                    {
                        Identifier = site.Id,
                        Label = site.Label,
                        Url = site.Url,
                        Regexp = site.BoundaryRegExp,
                        Tags = site.Tags.ToList(),
                        Active = site.IsActive
                    }
                };
                nodes.Add(node);
            }
        }
        if (nodes == null || !nodes.Any())
        {
            return new List<Node>();
        }
        return nodes;
    }

    }
}

