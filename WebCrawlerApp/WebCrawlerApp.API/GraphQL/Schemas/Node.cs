namespace WebCrawlerApp.API.GraphQL.Schemas {
    
    public class Node
    {
        public string? Title { get; set; }
        public string Url { get; set; }
        public Double CrawlTime { get; set; }
        public List<Node> Links { get; set; } = new List<Node>();
        public WebPage Owner { get; set; }
    }

}
