using WebCrawlerApp.Core.Interfaces;
using WebCrawlerApp.Infrastructure.Repositories;

namespace WebCrawlerApp.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public IWebsiteRepository WebsiteRepository { get; private set; }
        public IExecutionRepository ExecutionRepository { get; private set; }
        public ICrawlRepository CrawlRepository { get; private set; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            WebsiteRepository = new WebsiteRepository(context);
            ExecutionRepository = new ExecutionRepository(context);
            CrawlRepository = new CrawlRepository(context);
        }

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
