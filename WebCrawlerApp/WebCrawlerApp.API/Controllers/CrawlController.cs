using Microsoft.AspNetCore.Mvc;
using WebCrawlerApp.Application.Interfaces;

namespace WebCrawlerApp.API.Controllers;


[ApiController]
[Route("[controller]")]
public class CrawlController : ControllerBase
{
    private readonly ICrawlService _crawlService;
    private readonly IWebsiteService _websiteService;

    public CrawlController(ICrawlService crawlService, IWebsiteService websiteService)
    {
        _crawlService = crawlService;
        _websiteService = websiteService;
    }

    // POST: Start crawling a website
    [HttpPost("{websiteId}")]
    public async Task<IActionResult> Crawl(Guid websiteId)
    {
        var website = await _websiteService.GetWebsiteById(websiteId);
        if (website == null)
            return NotFound();

        var result = await _crawlService.CrawlWebsite(website.Data.Url, website.Data.BoundaryRegExp);
        return Ok(result);
    }
}
