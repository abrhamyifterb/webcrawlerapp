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

    /// <summary>
    /// Gets all active websites and checks if it's time to crawl them
    /// </summary>
    /// <returns></returns>
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


