namespace Shortha.Domain;

public class Tracker
{
    public string? BrowserName { get; set; }
    public string? BrowserVersion { get; set; }
    public string? OsName { get; set; }

    public string? Brand { get; set; }
    public string? Model { get; set; }
    public string? Device { get; set; }

    public string? UserAgent { get; set; }
    public string? UserId { get; set; }

    private string? _ipAddress;
    public bool IsBot { get; set; } = false;
    public string TimeZone { get; set; } = "Unknown";
    public string Country { get; set; } = "Unknown";
    public string Region { get; set; } = "Unknown";
    public string City { get; set; } = "Unknown";

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
