
using WebCrawlerApp.Core.Interfaces;
using WebCrawlerApp.Core.Entities;
using WebCrawlerApp.Application.Interfaces;
using WebCrawlerApp.Application.Dtos;

using AutoMapper;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;

namespace WebCrawlerApp.Application.Services 
{
    public class WebsiteService : IWebsiteService
    {
        private readonly IExecutionService _executionService;
        private readonly ICrawlService _crawlService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<WebsiteService> _logger;

        public WebsiteService(
                                IMapper mapper, 
                                IExecutionService executionService,
                                ICrawlService crawlService, 
                                IUnitOfWork unitOfWork,
                                ILogger<WebsiteService> logger
                            )
        {
            _executionService = executionService ?? throw new ArgumentNullException(nameof(executionService));
            _crawlService = crawlService ?? throw new ArgumentNullException(nameof(crawlService));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ResponseDTO<WebsiteDTO>> GetWebsiteById(Guid id)
        {
            var response = new ResponseDTO<WebsiteDTO>();
            try
            {
                var website = await _unitOfWork.WebsiteRepository.GetById(id);
                if (website == null)
                {
                    response.ErrorMessage = "Website not found.";
                    return response;
                }
                response.Data = _mapper.Map<WebsiteDTO>(website);
            } 
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching a website");
                response.ErrorMessage = "An internal error occurred.";
            }
            return response;
        }

        public async Task<ResponseDTO<IEnumerable<WebsiteDTO>>> GetAllWebsites()
        {
            var response = new ResponseDTO<IEnumerable<WebsiteDTO>>();
            try
            {
                var websites = await _unitOfWork.WebsiteRepository.GetAll();
                response.Data = _mapper.Map<IEnumerable<WebsiteDTO>>(websites);
            } 
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all websites");
                response.ErrorMessage = "An internal error occurred.";
            }
            return response;
        }

        public async Task<WebsiteDTO> CreateWebsite(WebsiteDTO websiteDto)
        {
            if (websiteDto == null) 
                throw new ArgumentNullException(nameof(websiteDto));

            var website = _mapper.Map<Website>(websiteDto);

            await _unitOfWork.WebsiteRepository.Add(website);

            var createdWebsite = _mapper.Map<WebsiteDTO>(website);
            return createdWebsite;
        }

        public async Task<WebsiteDTO> UpdateWebsite(WebsiteDTO websiteDto)
        {
            if (websiteDto == null) 
                throw new ArgumentNullException(nameof(websiteDto));
                
            var website = _mapper.Map<Website>(websiteDto);

            await _unitOfWork.WebsiteRepository.Update(website);

            var updtatedWebsite = _mapper.Map<WebsiteDTO>(website);
            return updtatedWebsite;
        }

        public async Task<bool> DeleteWebsite(Guid websiteId)
        {
            var website = await _unitOfWork.WebsiteRepository.GetById(websiteId);
            if (website != null)
            {
                // Delete all Execution and CrawledData associated with this Website.
                var executions = website.Executions;
                foreach (var execution in executions) {
                    var crawledData = await _unitOfWork.CrawlRepository.GetByExecutionId(execution.Id);
                    if (crawledData != null) {
                        await _unitOfWork.CrawlRepository.Delete(crawledData);
                    }
                    await _unitOfWork.ExecutionRepository.Delete(execution);
                }

                await _unitOfWork.WebsiteRepository.Delete(website);
                return true;
            }
            return false; 
        }

        public async Task<ResponseDTO<string>> TriggerCrawl(Guid websiteId)
        {
            var response = new ResponseDTO<string>();
            try
            {
                var website = await _unitOfWork.WebsiteRepository.GetById(websiteId);
                if (website == null)
                {
                    response.ErrorMessage = "Website not found.";
                    return response;
                }

                var startedExecution = await _executionService.StartExecution(websiteId, website.Label);
            
                var responseDTO = await _crawlService.CrawlWebsite(website.Url, website.BoundaryRegExp);
            
                if (responseDTO == null || responseDTO.Data == null) 
                {
                    _logger.LogWarning($"Failed to crawl website with ID: {websiteId}");
                    response.ErrorMessage = $"Failed to crawl website with ID: {websiteId}";
                    return response;
                }
                
                var crawledDataDto = responseDTO.Data;

                SetCrawledDatasExecutionId(crawledDataDto, startedExecution.Id);

                var crawledData = _mapper.Map<List<CrawledDataDTO>, List<CrawledData>>(crawledDataDto);
                
                await _unitOfWork.CrawlRepository.AddRange(crawledData);

                await ConvertUpdateCrawledData(website, crawledDataDto);

                await UpdateLastWebExecutionTime(website);
                
                await _executionService.EndExecution(startedExecution.Id, crawledData.Count());

                await _unitOfWork.CommitAsync();

                response.Data = "Crawl operation completed Successfully";
            } 
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while triggering the crawl.");
                response.ErrorMessage = "An internal error occurred.";
            }
            return response;
        }

        private void SetCrawledDatasExecutionId(List<CrawledDataDTO> crawledDataDTO, Guid executionId){
            foreach(var crawledData in crawledDataDTO)
            {
                crawledData.ExecutionId = executionId;
            }
        }
        private async Task UpdateLastWebExecutionTime(Website website) {
            website.UpdateLastExecutionTime(DateTime.UtcNow);
            await _unitOfWork.WebsiteRepository.Update(website);
        }

        private async Task ConvertUpdateCrawledData(Website website, List<CrawledDataDTO> crawledData) {
            var convertedJsonData = ConvertCrawledData(crawledData);
            website.UpdateCrawledData(convertedJsonData);
            await _unitOfWork.WebsiteRepository.Update(website);
        }
        private string ConvertCrawledData(ICollection<CrawledDataDTO> crawledData)
        {
            try 
            {
                return JsonConvert.SerializeObject(crawledData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to serialize crawled data.");
                return null;
            }
        }
    }

}