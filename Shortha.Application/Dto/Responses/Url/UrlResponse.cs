namespace Shortha.Application.Dto.Responses.Url
{
    public class UrlResponse
    {
        public required string Id { get; set; }
        public required string Url { get; set; }
        public required string ShortCode { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ExpiredAt { get; set; }
        public int ClickCount { get; set; }
        public bool IsActive { get; set; }
    }
}
