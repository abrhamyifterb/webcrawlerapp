namespace WebCrawlerApp.Application.Dtos
{
    /// <summary>
    /// Data Transfer Object for Execution 
    /// </summary>
    public class ExecutionDTO
    {
        public Guid Id { get; set; }
        public Guid WebsiteId { get; set; }
        public string WebsiteLabel { get; set; }
        public ExecutionStatus Status { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int? CrawledSitesCount { get; set; }
    }
}