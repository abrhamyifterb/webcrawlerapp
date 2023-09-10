using WebCrawlerApp.Core.Entities;

namespace WebCrawlerApp.Core.Interfaces 
{
    public interface IWebsiteRepository
    {
        Task<Website> GetById(Guid id);
        Task<IEnumerable<Website>> GetAll();
        Task<IEnumerable<Website>> GetActiveWebsites();
        Task<int> Add(Website website);
        Task<int> Update(Website website);
        Task<int> Delete(Website website);
    }
}

