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

    [HttpGet]
    public async Task<IActionResult> GetAllExecutions()
    {
        var execution = await _executionService.GetAll();
        if (execution == null) return NotFound();

        return Ok(execution);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetExecutionDetails(Guid id)
    {
        var execution = await _executionService.GetExecutionDetails(id);
        if (execution == null) return NotFound();

        return Ok(execution);
    }

    [HttpGet("website/{websiteId}")]
    public async Task<IActionResult> GetAllExecutionsForWebsite(Guid websiteId)
    {
        var executions = await _executionService.GetAllExecutionsForWebsite(websiteId);
        if (executions == null) return NotFound();
        return Ok(executions);
    }

}
