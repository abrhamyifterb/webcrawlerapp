public class PaginationFilter
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class SortingFilter
{
    public string SortBy { get; set; } = "Label"; // Default sort by Label
    public bool SortDescending { get; set; } = false;
}

public class WebsiteRecordFilter : PaginationFilter
{
    public string Url { get; set; }
    public string Label { get; set; }
    public List<string> Tags { get; set; } = new List<string>();
}
