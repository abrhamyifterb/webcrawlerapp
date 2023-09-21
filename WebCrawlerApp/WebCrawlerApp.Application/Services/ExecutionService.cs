using AutoMapper;
using WebCrawlerApp.Core.Entities;
using WebCrawlerApp.Core.Interfaces;
using WebCrawlerApp.Application.Interfaces;
using WebCrawlerApp.Application.Dtos;
using Microsoft.Extensions.Logging;

namespace WebCrawlerApp.Application.Services 
{
    public class ExecutionService : IExecutionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<ExecutionService> _logger;

        public ExecutionService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ExecutionService> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ResponseDTO<ExecutionDTO>> GetExecutionDetails(Guid executionId)
        {
            var response = new ResponseDTO<ExecutionDTO>();
            try
            {
                var execution = await _unitOfWork.ExecutionRepository.GetById(executionId);
                response.Data = _mapper.Map<ExecutionDTO>(execution);
            } 
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all executions");
                response.ErrorMessage = "An internal error occurred.";
            }
            return response;
        }

        public async Task<ResponseDTO<IEnumerable<ExecutionDTO>>> GetAllExecutionsForWebsite(Guid websiteId)
        {
            var response = new ResponseDTO<IEnumerable<ExecutionDTO>>();
            try
            {
                var executions = await _unitOfWork.ExecutionRepository.GetByWebsiteId(websiteId);
                response.Data = _mapper.Map<IEnumerable<ExecutionDTO>>(executions);
            } 
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all executions for a website");
                response.ErrorMessage = "An internal error occurred.";
            }
            return response;
        }
        public async Task<ResponseDTO<ExecutionDTO>> GetLatestExecutionForWebsites(List<Guid> websiteIds)
        {
            var response = new ResponseDTO<ExecutionDTO>();
            try
            {
                var execution = await _unitOfWork.ExecutionRepository.GetLatestExecutionByWebsiteIds(websiteIds);
                response.Data = _mapper.Map<ExecutionDTO>(execution);
            } 
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the latest execution for the websites");
                response.ErrorMessage = "An internal error occurred.";
            }
            return response;
        }

        public async Task<ResponseDTO<IEnumerable<ExecutionDTO>>> GetAll()
        {
            var response = new ResponseDTO<IEnumerable<ExecutionDTO>>();
            try
            {
                var executions = await _unitOfWork.ExecutionRepository.GetAll();
                response.Data = _mapper.Map<IEnumerable<ExecutionDTO>>(executions);
            } 
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all executions");
                response.ErrorMessage = "An internal error occurred.";
            }
            return response;
        }

        public async Task<ExecutionDTO> StartExecution(Guid websiteId, string websiteLabel)
        {
            var executionDto = new ExecutionDTO
            {
                Id = Guid.NewGuid(),
                WebsiteId = websiteId,
                Status = ExecutionStatus.Started,
                StartTime = DateTime.Now,
                WebsiteLabel = websiteLabel
            };
            var execution = _mapper.Map<Execution>(executionDto);

            await _unitOfWork.ExecutionRepository.Add(execution);
            
            await _unitOfWork.CommitAsync();
            Console.WriteLine(" -----------------------------");
            Console.WriteLine(executionDto.Id);
            Console.WriteLine(" Start of Execution ");
            Console.WriteLine(" -----------------------------");
            return _mapper.Map<ExecutionDTO>(execution);
        }

        public async Task<ExecutionDTO> EndExecution(Guid executionId, int crawledSitesCount)
        {
            var execution = await _unitOfWork.ExecutionRepository.GetById(executionId);
            if (execution == null) return null;

            execution.SetCrawledSitesCount(crawledSitesCount);
            execution.CompleteExecution();

            await _unitOfWork.ExecutionRepository.Update(execution);
            Console.WriteLine(executionId);
            Console.WriteLine(" End of Execution ");
            Console.WriteLine(" -----------------------------");
            return _mapper.Map<ExecutionDTO>(execution);
        }

    }
}