

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebCrawlerApp.Core.Entities {
    public class CrawledData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Url { get; private set; }
        public string Title { get; private set; }
        public Double CrawlTime { get; private set; }
        public bool IsRestricted { get; private set; }
        public ICollection<string> Links { get; private set; } = new List<string>();
        public Guid ExecutionId { get; private set; }
        public virtual Execution Execution { get; set; } 

        protected CrawledData() {} 

        public CrawledData(string url, string title, ICollection<string> links)
        {
            SetUrl(url);
            SetTitle(title);
            SetLinks(links);
        }
        public void SetUrl(string url)
        {
            if(string.IsNullOrWhiteSpace(url))
                throw new ArgumentException("Url cannot be null or empty.");

            Url = url;
        }

        public void SetTitle(string title)
        {
            if(string.IsNullOrWhiteSpace(title)) 
                title = "No Title Provided"; 

            Title = title;
        }
        public void SetIsRestricted(bool isRestricted)
        {
            IsRestricted = isRestricted;
        }
        public void SetExecution(Guid excutionId){
            ExecutionId = excutionId;
        }
        public void SetLinks(ICollection<string> links)
        {
            Links = links;
        }
    }
}

