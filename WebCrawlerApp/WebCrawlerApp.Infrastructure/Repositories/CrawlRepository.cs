using WebCrawlerApp.Core.Interfaces;
using WebCrawlerApp.Core.Entities;
using WebCrawlerApp.Infrastructure.Data;

namespace WebCrawlerApp.Infrastructure.Repositories
{
    public class CrawlRepository : ICrawlRepository
    {
        private readonly AppDbContext _context;

        public CrawlRepository(AppDbContext context)
        {
            _context = context;
        }

        Task<CrawledData> ICrawlRepository.CrawlWebsite(string url, string boundaryRegExp)
        {
            throw new NotImplementedException();
        }

        public async Task<int> AddRange(IEnumerable<CrawledData> crawledData)
        {
            await _context.CrawledDatas.AddRangeAsync(crawledData);
            return await _context.SaveChangesAsync();
        }


        async Task<int> ICrawlRepository.Add(CrawledData crawledData)
        {
            await _context.CrawledDatas.AddAsync(crawledData);
            
            return await _context.SaveChangesAsync();
        }
    }
}
