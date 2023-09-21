using WebCrawlerApp.Core.Entities;

namespace WebCrawlerApp.Core.Interfaces 
{
    public interface IExecutionRepository
    {
        Task<Execution> GetById(Guid id);
        Task<IEnumerable<Execution>> GetByWebsiteId(Guid id);
        Task<IEnumerable<Execution>> GetAll();
        Task<Execution> GetLatestExecutionByWebsiteIds(List<Guid> websiteIds);
        Task<int> Add(Execution website);
        Task<int> Update(Execution website);
        Task<int> Delete(Execution website);  
    }
}
