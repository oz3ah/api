using DeviceDetectorNET;
using Shortha.Domain;

namespace Shortha.Application;

public class TrackerBuilder
{
    private readonly Tracker _tracker = new();
    private readonly DeviceDetector _deviceDetector;

    public TrackerBuilder(string userAgent)
    {
        _deviceDetector = new DeviceDetector(userAgent);
        _deviceDetector.Parse();
    }

    public TrackerBuilder WithIp(string ip)
    {
        _tracker.IpAddress = ip;
        return this;
    }


    public TrackerBuilder WithOs()
    {
        var os = _deviceDetector.GetOs();
        if (os?.Match != null)
        {
            _tracker.OsName = os.Match.Name;
        }
        else
        {
            _tracker.OsName = "Unknown";
        }

        return this;
    }

    public TrackerBuilder WithBrowser()
    {
        var client = _deviceDetector.GetClient();
        if (client?.Match != null)
        {
            _tracker.BrowserName = client.Match.Name;
            _tracker.BrowserVersion = client.Match.Version;
        }
        else
        {
            _tracker.BrowserName = "Unknown";
            _tracker.BrowserVersion = "Unknown";
        }

        return this;
    }

    public TrackerBuilder WithDevice()
    {
        _tracker.Device = _deviceDetector.GetDeviceName();
        return this;
    }

    public TrackerBuilder WithBrand()
    {
        _tracker.Brand = _deviceDetector.GetBrandName();
        return this;
    }

    public TrackerBuilder WithModel()
    {
        _tracker.Model = _deviceDetector.GetModel();
        return this;
    }

    public Tracker Build()
    {
        return _tracker;
    }
}
