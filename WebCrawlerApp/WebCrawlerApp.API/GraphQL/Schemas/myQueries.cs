using Newtonsoft.Json;
using System.Diagnostics;
using System.Threading.Tasks;
using WebCrawlerApp.Core.Entities;
using WebCrawlerApp.Infrastructure.Data;

namespace WebCrawlerApp.API.GraphQL.Schemas
{
    public partial class Query
    {
        public IEnumerable<WebPage> GetWebsites([Service] AppDbContext _context)
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

        public List<WebNode> GetWebNodesByIds([Service] AppDbContext _context, List<Guid> webPageIds)
        {
            var filteredWebPages = _context.Websites
                .Where(w => webPageIds.Contains(w.Id)).ToList();

            var result = new List<WebNode>();
            foreach (var f in filteredWebPages)
            {
                var crawledDataList = JsonConvert.DeserializeObject<List<CrawledData>>(f.CrawledData);
                foreach (var c in crawledDataList) 
                {
                    var myNode = new WebNode();
                    myNode.Url = c.Url;
                    myNode.Title = c.Title;
                    myNode.CrawlTime = c.CrawlTime;

                    var owner = new WebPage();
                    owner.Identifier = f.Id;
                    owner.Url = f.Url;
                    owner.Tags = (List<string>)f.Tags;
                    owner.Active = f.IsActive;

                    myNode.Owner = owner;
                    result.Add(myNode);

                }
            }

            return result;
        }


        public WebNode GetWebNodesWithoutId([Service] AppDbContext _context)
        {

            var crawledDataList = _context.Websites
                .Select(w => w.CrawledData);
            var returnthis = crawledDataList.FirstOrDefault();

            var taso = new WebNode();
            taso.Title = "taso";
            return taso;


        }

    }
}
