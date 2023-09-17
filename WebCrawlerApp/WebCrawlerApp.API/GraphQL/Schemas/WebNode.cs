public class WebNode
{

    public string Title { get; set; }
    public string? Url { get; set; }
    public TimeSpan CrawlTime { get; set; }
    public List<WebNode>? Links { get; set; }

    //public WebPageType? Owner { get; set; }

}