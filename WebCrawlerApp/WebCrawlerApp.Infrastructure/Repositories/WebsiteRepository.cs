using System.Collections.Generic;
using System.Linq;
using System;
using WebCrawlerApp.Core.Interfaces;
using WebCrawlerApp.Core.Entities;
using WebCrawlerApp.Infrastructure.Data;

namespace WebCrawlerApp.Infrastructure.Repositories
{
    public class WebsiteRepository : IWebsiteRepository
    {
        private readonly AppDbContext _context;

        public WebsiteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Website> GetById(Guid id)
        {
            var result = await _context.Websites.FindAsync(id);
            return result;
        }

        Task<int> IWebsiteRepository.Add(Website website)
        {
            _context.Websites.Add(website);
            
            return Task.FromResult(_context.SaveChanges());
        }

        Task<int> IWebsiteRepository.Delete(Website website)
        {
            _context.Websites.Remove(website);
            return Task.FromResult(_context.SaveChanges());
        }

        Task<IEnumerable<Website>> IWebsiteRepository.GetAll()
        {
            var myWebsiteList = _context.Websites.ToList();
            return Task.FromResult(myWebsiteList as IEnumerable<Website>);
        }

        Task<IEnumerable<Website>> IWebsiteRepository.GetActiveWebsites()
        {
            var myWebsiteList = _context.Websites
                .Where(e => e.IsActive == true)
                .ToList();
            return Task.FromResult(myWebsiteList as IEnumerable<Website>);
        }

        Task<int> IWebsiteRepository.Update(Website website)
        {
            _context.Websites.Update(website);
            return Task.FromResult(_context.SaveChanges());
        }
    }
}
