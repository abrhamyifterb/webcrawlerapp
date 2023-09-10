using WebCrawlerApp.Application.Interfaces;
using WebCrawlerApp.Core.Entities;
using WebCrawlerApp.Core.Interfaces;

public class ExecutionScheduler
{
    private readonly IWebsiteRepository _websiteRepo;
    
    private readonly IWebsiteService _websiteService;

    public ExecutionScheduler(IWebsiteRepository websiteRepo, IWebsiteService websiteService)
    {
        _websiteRepo = websiteRepo;
        _websiteService = websiteService;
    }

    public async Task CheckAndExecutePendingWebsites()
    {
        var activeWebsites = await _websiteRepo.GetActiveWebsites();

        var tasks = new List<Task>();

        foreach (var website in activeWebsites)
        {
            if (ShouldExecute(website))
            {
                tasks.Add(Task.Run(async () =>
                {
                    await _websiteService.TriggerCrawl(website.Id);
                }));
            }
        }

        await Task.WhenAll(tasks);
    }


    private bool ShouldExecute(Website website)
    {
        var nowUtc = DateTime.UtcNow;
        return nowUtc >= website.LastExecutionTime.AddMinutes(website.CrawlFrequency);
    }

}
















// using Microsoft.Extensions.DependencyInjection;
// using WebCrawlerApp.Application.Interfaces;
// using WebCrawlerApp.Core.Entities;
// using WebCrawlerApp.Core.Interfaces;

// public class ExecutionScheduler
// {
//     private readonly IServiceScopeFactory _scopeFactory;

//     public ExecutionScheduler(IServiceScopeFactory scopeFactory)
//     {
//         _scopeFactory = scopeFactory;
//     }

//     public async Task CheckAndExecutePendingWebsites()
//     {
//         var activeWebsites = await GetActiveWebsites();

//         var tasks = activeWebsites.Where(ShouldExecute).Select(website => TriggerCrawl(website.Id)).ToList();

//         await Task.WhenAll(tasks);
//     }

//     private async Task<IEnumerable<Website>> GetActiveWebsites()
//     {
//         using var scope = _scopeFactory.CreateScope();
//         var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
//         return await unitOfWork.WebsiteRepository.GetActiveWebsites();
//     }

//     private async Task TriggerCrawl(Guid websiteId)
//     {
//         using var scope = _scopeFactory.CreateScope();
//         var websiteService = scope.ServiceProvider.GetRequiredService<IWebsiteService>();
//         await websiteService.TriggerCrawl(websiteId);
//     }
//     private bool ShouldExecute(Website website)
//     {
//         var nowUtc = DateTime.UtcNow;
//         return nowUtc >= website.LastExecutionTime.AddMinutes(website.CrawlFrequency);
//     }

// }
