using Shortha.Domain;

namespace Shortha.Infrastructre;

using DeviceDetectorNET;
using IPinfo;
using Shortha.Domain.Dto;


public class TrackerBuilder
{
    private readonly DeviceDetector _deviceDetector;
    private readonly Tracker _tracker;
    private readonly IPinfoClient _client;

    public TrackerBuilder(string userAgent,  IPinfoClient client)
    {
        _deviceDetector = new DeviceDetector(userAgent);
        _deviceDetector.Parse();

        _client = client;

        _tracker = new Tracker
        {
            UserAgent = userAgent
        };
    }

    public TrackerBuilder WithBrowser()
    {
        _tracker.BrowserName = _deviceDetector.GetClient().Match.Name;
        _tracker.BrowserVersion = _deviceDetector.GetClient().Match.Version;
        return this;
    }

    public TrackerBuilder WithOs()
    {
        _tracker.OsName = _deviceDetector.GetOs().Match.Name;
        return this;
    }

    public TrackerBuilder WithBrand()
    {
        _tracker.DeviceBrand = _deviceDetector.GetBrandName();
        return this;
    }

    public TrackerBuilder WithModel()
    {
        _tracker.DeviceType = _deviceDetector.GetModel();
        return this;
    }

    public TrackerBuilder WithIpAddress(string ip)
    {
        var ipInfo = _client.IPApi.GetDetails(ip);
        if (ipInfo != null)
        {
            _tracker.IpAddress = ipInfo.IP;
            _tracker.Country = ipInfo.Country;
            _tracker.Region = ipInfo.Region;
            _tracker.City = ipInfo.City;
        }

        return this;
    }

    public TrackerBuilder WithIsBot()
    {
        _tracker.IsBot = _deviceDetector.IsBot();
        return this;
    }

    public TrackerBuilder WithTimeZone()
    {
        if (_tracker.IpAddress != "Unknown")
        {
            var ipInfo = _client.IPApi.GetDetails(_tracker.IpAddress);
            if (ipInfo != null)
            {
                _tracker.TimeZone = ipInfo.Timezone;
            }
        }

        return this;
    }


    public Tracker Build()
    {
        return _tracker;
    }
}