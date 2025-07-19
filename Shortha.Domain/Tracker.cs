namespace Shortha.Domain;

public class Tracker
{
    public string?BrowserName { get; set; }
    public string?BrowserVersion { get; set; }
    public string?OsName { get; set; }
    public string?DeviceBrand { get; set; }
    public string?DeviceType { get; set; }
    public string?UserAgent { get; set; }

    private string? _ipAddress;
    public bool IsBot { get; set; } = false;
    public string TimeZone { get; set; } = "Unknown";
    public string Country { get; set; }
    public string Region { get; set; }
    public string City { get; set; }

    public string IpAddress
    {
        get
        {
            if (string.IsNullOrEmpty(_ipAddress) || _ipAddress == "::1")
            {
                return "Unknown";
            }

            return _ipAddress;
        }
        set { _ipAddress = value; }
    }
}