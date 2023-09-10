
// using WebCrawlerApp.Core.Entities;
// using WebCrawlerApp.Infrastructure.Data;

// namespace WebCrawlerApp.API.GraphQL
// {
//     public class Query
//     {
//         [UseProjection]
//         [UseFiltering]
//         [UseSorting]
//         public IQueryable<Website> GetWebsites([Service] AppDbContext dbContext) =>
//             dbContext.Websites;

//            // new List<Website>().AsQueryable();

//         // public List<WebPage> Websites([Service] AppDbContext dbContext)
//         // {
//         //     return dbContext.Websites
//         //                     .Select(w => new WebPage {
//         //                         Identifier = w.Id,
//         //                         Label = w.Label,
//         //                         Url = w.Url,
//         //                         Regexp = w.BoundaryRegExp,
//         //                         Tags = w.Tags.ToList(),
//         //                         Active = w.IsActive
//         //                     })
//         //                     .ToList();
//         // }

//         // public List<Node> Nodes([Service] AppDbContext dbContext,List<Guid> webPages)
//         // {
//         //     var crawledData = dbContext.Websites
//         //                             .Where(w => webPages.Contains(w.Id))
//         //                             .SelectMany(w => JsonConvert.DeserializeObject<List<CrawledNode>>(w.CrawledData))
//         //                             .ToList();

//         //     return crawledData.Select(data => MapToNode(data)).ToList();
//         // }

//         // private Node MapToNode(CrawledNode data)
//         // {
//         //     return new Node {
//         //         Title = data.Title,
//         //         Url = data.Url,
//         //         CrawlTime = data.CrawlTime,
//         //         Links = data.Links.Select(link => new Node { Url = link }).ToList(),
//         //         Owner = dbContext.Websites
//         //                         .Where(w => w.Executions.Any(e => e.Id == data.ExecutionId))
//         //                         .Select(w => new WebPage {
//         //                             Identifier = w.Id,
//         //                             Label = w.Label,
//         //                             Url = w.Url,
//         //                             Regexp = w.BoundaryRegExp,
//         //                             Tags = w.Tags.ToList(),
//         //                             Active = w.IsActive
//         //                         })
//         //                         .FirstOrDefault()
//         //     };
//         // }
   
//     }
// }

using Newtonsoft.Json;
using WebCrawlerApp.Core.Entities;
using WebCrawlerApp.Infrastructure.Data;

using Newtonsoft.Json;
using System.Linq;

namespace WebCrawlerApp.API.GraphQL
{
public class Query
{

    private readonly AppDbContext _context; // Assume you have a DbContext named YourDbContext

    public Query(AppDbContext context)
    {
        _context = context;
    }

    // [UseProjection]
    // [UseFiltering]
    // [UseSorting]
    // public IQueryable<Website> GetWebsites([Service] AppDbContext dbContext) =>
    //          dbContext.Websites;



    public IEnumerable<WebPage> GetWebsites()
    {
        //return _context.Websites;
        return _context.Websites.Select(w => new WebPage
        {
            Identifier = w.Id,
            Label = w.Label,
            Url = w.Url,
            Regexp = w.BoundaryRegExp,
            Tags = w.Tags.ToList(),
            Active = w.IsActive
        }).ToList();
    }

    // public IEnumerable<Node> GetNodes(List<Guid> webPageIds)
    // {
    //     var crawledDataList = _context.Websites
    //         .Where(w => webPageIds.Contains(w.Id))
    //         .Select(w => JsonConvert.DeserializeObject<List<Node>>(w.CrawledData)) // Deserializing the CrawledData to get Node data
    //         .ToList();

    //     return crawledDataList.SelectMany(data => data).ToList();
    // }

    // public IQueryable<Website> GetWebsites([Service] AppDbContext dbContext)
    // {
    //     return dbContext.Websites;
    // }
    // public async Task<IEnumerable<Node>> GetNodes(List<Guid> webPages, [Service] AppDbContext dbContext)
    // {
    //     var websites = dbContext.Websites.Where(w => webPages.Contains(w.Id)).ToList();

    //     List<Node> nodes = new List<Node>();
    //     foreach (var website in websites)
    //     {
    //         var crawledData = JsonConvert.DeserializeObject<List<Node>>(website.CrawledData);
    //         nodes.AddRange(crawledData);
    //     }

    //     return nodes;
    // }
}
}