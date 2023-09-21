using Microsoft.AspNetCore.Mvc;
using WebCrawlerApp.Application.Interfaces;
namespace WebCrawlerApp.API.Controllers;


[ApiController]
[Route("[controller]")]
public class ExecutionController : ControllerBase
{
    private readonly IExecutionService _executionService;

    public ExecutionController(IExecutionService executionService)
    {
        _executionService = executionService;
    }

    /// <summary>
    /// Gets all Execution records 
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetAllExecutions()
    {
        var execution = await _executionService.GetAll();
        if (execution == null) return NotFound();

        return Ok(execution);
    }

    /// <summary>
    /// Gets Execution records based on their ids
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetExecutionDetails(Guid id)
    {
        var execution = await _executionService.GetExecutionDetails(id);
        if (execution == null) return NotFound();

        return Ok(execution);
    }

    /// <summary>
    /// Gets all executions based on the corresponding webside ids
    /// </summary>
    /// <param name="websiteId"></param>
    /// <returns></returns>
    [HttpGet("website/{websiteId}")]
    public async Task<IActionResult> GetAllExecutionsForWebsite(Guid websiteId)
    {
        var executions = await _executionService.GetAllExecutionsForWebsite(websiteId);
        if (executions == null) return NotFound();
        return Ok(executions);
    }

    /// <summary>
    /// Gets the latest execution for the corresponding websites
    /// </summary>
    /// <param name="websiteIds"></param>
    /// <returns></returns>
    [HttpGet("websites/latest")]
    public async Task<IActionResult> GetLatestExecutionForWebsites([FromQuery] List<Guid> websiteIds)
    {
        var execution = await _executionService.GetLatestExecutionForWebsites(websiteIds);
        if (execution.Data == null) return NotFound();
        return Ok(execution);
    }

}
