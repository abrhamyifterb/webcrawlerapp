using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebCrawlerApp.Core.Entities {
    public class Website
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; private set; }
        public string Url { get; private set; }
        public string BoundaryRegExp { get; private set; }
        public int CrawlFrequency { get; private set; }
        public string Label { get; private set; }
        public bool IsActive { get; private set; }
        public ICollection<string> Tags { get; private set; } = new List<string>();
        public DateTime LastExecutionTime { get; private set; }
        public ICollection<Execution> Executions { get; private set; } = new List<Execution>();
        public string? CrawledData { get; private set; }


        protected Website() {}

        public Website(string url, string boundaryRegExp, int crawlFrequency, 
                    string label, bool isActive, ICollection<string> tags, string crawledData)
        {
            SetUrl(url);
            SetBoundaryRegExp(boundaryRegExp);
            SetCrawlFrequency(crawlFrequency);
            SetLabel(label);
            SetIsActive(isActive);
            SetTags(tags);
            SetCrawledData(crawledData);
        }

        public void SetUrl(string url)
        {
            Url = url ?? throw new ArgumentException("Url cannot be null or empty.");
        }
        public void SetBoundaryRegExp(string boundaryRegExp)
        {
            if(string.IsNullOrWhiteSpace(boundaryRegExp))
                throw new ArgumentException("Boundary RegExp cannot be null or empty.");

            BoundaryRegExp = boundaryRegExp;
        }

        public void SetCrawlFrequency(int crawlFrequency)
        {
            CrawlFrequency = crawlFrequency;
        }

        public void SetLabel(string label)
        {
            if(string.IsNullOrWhiteSpace(label))
                throw new ArgumentException("Label cannot be null or empty.");

            Label = label;
        }

        public void SetIsActive(bool isActive)
        {
            IsActive = isActive;
        }

        public void SetTags(ICollection<string> tags)
        {
            if(tags == null || !tags.Any())
                throw new ArgumentException("Tags cannot be null or empty.");

            Tags = tags;
        }
        public void SetCrawledData(string crawledData)
        {
            // if(tags == null || !tags.Any())
            //     throw new ArgumentException("Tags cannot be null or empty.");

            CrawledData = crawledData;
        }

        public void UpdateLastExecutionTime(DateTime lastExecutionTime)
        {
            LastExecutionTime = lastExecutionTime;
        }

        public void UpdateCrawledData(string crawledData)
        {
            CrawledData = crawledData;
        }
    }
}

