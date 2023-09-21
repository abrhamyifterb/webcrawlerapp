namespace WebCrawlerApp.API.GraphQL.Schemas {
    public class WebPage
    {
        public Guid Identifier { get; set; }
        public string Label { get; set; }
        public string Url { get; set; }
        public string Regexp { get; set; }
        public List<string> Tags { get; set; }
        public bool Active { get; set; }
    }
}