public class Node
{
    public string Title { get; set; }
    public string Url { get; set; }
    public TimeSpan CrawlTime { get; set; }
    public List<Node> Links { get; set; }
    public WebPage Owner { get; set; }
}

// public class Node
// {
//     public string? Title { get; set; }
//     public string Url { get; set; }
//     public DateTime? CrawlTime { get; set; }
//     public List<Node> Links { get; set; }
//     public WebPage Owner { get; set; }
// }

// public class Node
// {
//     public Guid Id { get; set; }
//     public string Url { get; set; }
//     public string Title { get; set; }
//     public TimeSpan CrawlTime { get; set; }
//     public DateTime StartTime { get; set; }
//     public bool IsRestricted { get; set; }
//     public ICollection<string> Links { get; set; } = new List<string>();
//     public Guid ExecutionId { get; set; }
// }
