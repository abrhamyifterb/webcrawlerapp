using WebCrawlerApp.Core.Entities;
using System;
using System.Threading.Tasks;

namespace WebCrawlerApp.Core.Interfaces 
{
    public interface ICrawlRepository
    {
        Task<int> Add(CrawledData crawledData);
		Task<int> Delete(CrawledData crawledData);

		Task<CrawledData> CrawlWebsite(string url, string boundaryRegExp);
        Task<int> AddRange(IEnumerable<CrawledData> crawledData);
		Task<CrawledData> GetByExecutionId(Guid id);
	}
}