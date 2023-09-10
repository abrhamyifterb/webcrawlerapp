namespace WebCrawlerApp.Application.Dtos
{
    public class ResponseDTO<T>
    {
        public T Data { get; set; }
        public string ErrorMessage { get; set; }
        public bool HasError => !string.IsNullOrEmpty(ErrorMessage);
    }
}