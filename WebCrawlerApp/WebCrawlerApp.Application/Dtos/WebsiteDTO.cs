namespace WebCrawlerApp.Application.Dtos
{
    /// <summary>
    /// Data Transfer Object for Website 
    /// </summary>
    public class WebsiteDTO
    {
        public Guid? Id { get; set; }
        public string Url { get; set; }
        public string BoundaryRegExp { get; set; }
        public int CrawlFrequency { get; set; }
        public string Label { get; set; }
        public bool IsActive { get; set; }
        public List<string> Tags { get; set; }
        public DateTime? LastExecutionTime { get; set; }
        public string CrawledData { get; set; }
    }
}