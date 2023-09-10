using WebCrawlerApp.Application.Dtos;

namespace WebCrawlerApp.Application.Interfaces
{
    public interface ICrawlService
    {
        public Task<ResponseDTO<List<CrawledDataDTO>>> CrawlWebsite(string url, string boundaryRegExp);
    }
}