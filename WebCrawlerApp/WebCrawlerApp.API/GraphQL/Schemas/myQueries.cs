using Newtonsoft.Json;
using System.Diagnostics;
using System.Threading.Tasks;
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
            var crawledDataList = _context.Websites
                .Where(w => webPageIds.Contains(w.Id))
                .Select(w => w.CrawledData).ToList();

            var deserializedList = new List<WebNode>();

            foreach (var crawledData in crawledDataList)
            {
                var deserializedData = JsonConvert.DeserializeObject<List<WebNode>>(crawledData);
                deserializedList.AddRange(deserializedData);
                Debug.WriteLine("taso");
            }

            return deserializedList;

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
