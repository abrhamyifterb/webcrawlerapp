namespace WebCrawlerApp.Application.Dtos
{
    public class CrawledDataDTO
    {
        public Guid Id { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public Double CrawlTime { get; set; }
        public bool IsRestricted { get; set; }
        public ICollection<string> Links { get; set; }
        public Guid? ExecutionId { get; set; }
    }
}
