using System;

namespace WebCrawlerApp.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IWebsiteRepository WebsiteRepository { get; }
        IExecutionRepository ExecutionRepository { get; }
        ICrawlRepository CrawlRepository { get; }
        Task<int> CommitAsync();
    }
}
