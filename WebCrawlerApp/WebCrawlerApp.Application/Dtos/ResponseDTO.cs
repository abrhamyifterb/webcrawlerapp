namespace WebCrawlerApp.Application.Dtos
{
    /// <summary>
    /// Data Transfer Object for Response for the frontend
    /// Consists of other dto and errormessage
    /// </summary>
    public class ResponseDTO<T>
    {
        public T Data { get; set; }
        public string ErrorMessage { get; set; }
        public bool HasError => !string.IsNullOrEmpty(ErrorMessage);
    }
}