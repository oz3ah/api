namespace Shortha.Application.Dto.Responses.Visit
{
    public class VisitsResponse
    {
        public string Id { get; set; }
        public string Browser { get; set; } = string.Empty;
        public string Os { get; set; } = string.Empty;
        public string DeviceBrand { get; set; } = string.Empty;
        public string DeviceType { get; set; } = string.Empty;
        public string IpAddress { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public DateTime VisitDate { get; set; }

    }
}
