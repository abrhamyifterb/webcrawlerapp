using Microsoft.AspNetCore.Mvc;
using WebCrawlerApp.Application.Interfaces;
using WebCrawlerApp.Application.Dtos;
namespace WebCrawlerApp.API.Controllers;


[ApiController]
[Route("[controller]")]
public class WebsiteController : ControllerBase
{
    private readonly IWebsiteService _websiteService;

    public WebsiteController(IWebsiteService websiteService)
    {
        _websiteService = websiteService;
    }

    /// <summary>
    /// Gets all websites
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var allSites = await _websiteService.GetAllWebsites();
        return Ok(allSites);
    }

    /// <summary>
    /// Gets website based on the id 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var website = await _websiteService.GetWebsiteById(id);
        if (website == null)
            return NotFound();

        return Ok(website);
    }

    /// <summary>
    /// Creates Website
    /// </summary>
    /// <param name="website"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] WebsiteDTO website)
    {
        var addedWebsite = await _websiteService.CreateWebsite(website);
        return CreatedAtAction(nameof(Get), new { id = addedWebsite.Id }, addedWebsite);
    }

    /// <summary>
    /// Updates Website
    /// </summary>
    /// <param name="website"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> Put([FromBody] WebsiteDTO website)
    {
        var updatedWebsite = await _websiteService.UpdateWebsite(website);
        if (updatedWebsite == null)
            return NotFound();

        return Ok(updatedWebsite);
    }

    /// <summary>
    /// Deletes website
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _websiteService.DeleteWebsite(id);
        if (!result)
            return NotFound();

        return NoContent();
    }

    /// <summary>
    /// Triggers the crawling process based on the website id that is passed
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPost("{id}/crawl")]
    public async Task<IActionResult> TriggerCrawl(Guid id)
    {
        await _websiteService.TriggerCrawl(id);
        return Ok();
    }
}
