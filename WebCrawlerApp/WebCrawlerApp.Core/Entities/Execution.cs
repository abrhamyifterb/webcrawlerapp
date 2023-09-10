using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebCrawlerApp.Core.Entities {
    public class Execution
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid WebsiteId { get; set; }
        public virtual Website Website { get; set; } 
        public string WebsiteLabel { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; } 
        public ExecutionStatus Status { get; set; }
        public int CrawledSitesCount { get; set; }

        protected Execution() {} 

        public Execution(Website website)
        {
            Id = Guid.NewGuid();
            SetWebsite(website);
            StartTime = DateTime.UtcNow;
            Status = ExecutionStatus.Started;
        }

        public void SetWebsite(Website website)
        {
            Website = website ?? throw new ArgumentNullException(nameof(website));
            WebsiteId = website.Id;
        }

        public void CompleteExecution()
        {
            EndTime = DateTime.UtcNow;
            Status = ExecutionStatus.Completed;
        }
        public void UpdateEndTime(DateTime endTime)
        {
            EndTime = endTime;
        }

        public void UpdateStatus(ExecutionStatus status)
        {
            Status = status;
        }

        public void SetCrawledSitesCount(int crawledSitesCount) {
            CrawledSitesCount = crawledSitesCount;
        }
    }
}