using WebCrawlerApp.Core.Entities;
using WebCrawlerApp.Application.Dtos;

namespace WebCrawlerApp.Application.Interfaces 
{
    public interface IExecutionService
    {
        Task<ResponseDTO<ExecutionDTO>> GetExecutionDetails(Guid executionId);
        Task<ResponseDTO<IEnumerable<ExecutionDTO>>> GetAllExecutionsForWebsite(Guid websiteId);
        Task<ResponseDTO<IEnumerable<ExecutionDTO>>> GetAll();
        Task<ExecutionDTO> StartExecution(Guid websiteId, string websiteLabel);
        Task<ExecutionDTO> EndExecution(Guid executionId, int crawledSitesCount);
    }
}