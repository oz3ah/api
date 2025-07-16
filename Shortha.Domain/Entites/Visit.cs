namespace Shortha.Domain.Entites
{
    public class Visit
    {
        public string Id { get; set; }
        public DateTime VisitDate { get; set; } = DateTime.UtcNow;
        public string? UserAgent { get; set; }
        public string? IpAddress { get; set; }
        public string? Referrer { get; set; }
        public string? Country { get; set; }
        public string? Region { get; set; }
        public string? City { get; set; }
        public string? DeviceBrand { get; set; }
        public string? DeviceType { get; set; }
        public string? Browser { get; set; }
        public string? Os { get; set; }
        public bool IsBot { get; set; } = false;
        public string? Language { get; set; }
        public string? TimeZone { get; set; }

        public string UrlId { get; set; }
        public virtual Url Url { get; set; } = null!;

        // Helper properties for analytics
        public string DeviceInfo => $"{DeviceBrand} {DeviceType}".Trim();
        public string LocationInfo => $"{City}, {Region}, {Country}".Replace(", ,", ",").Trim(',', ' ');
        public bool IsUnique => !string.IsNullOrEmpty(IpAddress);


        public Visit()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}