using WebCrawlerApp.Core.Interfaces;
using WebCrawlerApp.Core.Entities;
using WebCrawlerApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace WebCrawlerApp.Infrastructure.Repositories
{
    public class ExecutionRepository : IExecutionRepository
    {
        private readonly AppDbContext _context;

        public ExecutionRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task<Execution> GetById(Guid id)
        {
            var result = _context.Executions.Find(id);
            return Task.FromResult(result);
        }

        async Task<int> IExecutionRepository.Add(Execution execution)
        {
            await _context.Executions.AddAsync(execution);
            
            return await _context.SaveChangesAsync();
        }

        Task<int> IExecutionRepository.Delete(Execution execution)
        {
            _context.Executions.Remove(execution);
            return Task.FromResult(_context.SaveChanges());
        }

        Task<IEnumerable<Execution>> IExecutionRepository.GetAll()
        {
            var myExecutionList = _context.Executions.ToList();
            return Task.FromResult(myExecutionList as IEnumerable<Execution>);
        }

        Task<IEnumerable<Execution>> IExecutionRepository.GetByWebsiteId(Guid websiteId)
        {
            var myExecutionList = _context.Executions
                .Where(e => e.WebsiteId == websiteId)
                .ToList();
            return Task.FromResult(myExecutionList as IEnumerable<Execution>);
        }

        async Task<int> IExecutionRepository.Update(Execution execution)
        {
            _context.Executions.Update(execution);
            return await _context.SaveChangesAsync();
        }
    }
}
