using WebCrawlerApp.Application.Dtos;

namespace WebCrawlerApp.Application.Interfaces
{
    public interface IWebsiteService
    {
        Task<ResponseDTO<WebsiteDTO>> GetWebsiteById(Guid id);
        Task<ResponseDTO<IEnumerable<WebsiteDTO>>> GetAllWebsites();
        Task<WebsiteDTO> CreateWebsite(WebsiteDTO websiteDto);
        Task<WebsiteDTO> UpdateWebsite(WebsiteDTO websiteDto);
        Task<bool> DeleteWebsite(Guid websiteId);
        Task<ResponseDTO<string>> TriggerCrawl(Guid websiteId);
    }
}