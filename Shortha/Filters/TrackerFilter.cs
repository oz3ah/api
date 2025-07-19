using IPinfo;
using Microsoft.AspNetCore.Mvc.Filters;
using Shortha.Application.Exceptions;
using Shortha.Infrastructre;

namespace Shortha.Filters;

public class TrackerFilter(IPinfoClient client, ILogger<Serilog.ILogger> logger) : IActionFilter
{

    public void OnActionExecuting(ActionExecutingContext context)
    {
        var query = context.HttpContext.Request.Query;
        var hash = query["hash"].ToString();
        var fingerprint = query["fingerprint"].ToString();
        var userAgent = context.HttpContext.Request.Headers["User-Agent"].ToString();
        var ipAddress = context.HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();

        logger.LogInformation("TrackerFilter: UserAgent: {UserAgent}, IP: {IpAddress}, Hash: {Hash}, Fingerprint: {Fingerprint}",
            userAgent, ipAddress, hash, fingerprint);

        if (string.IsNullOrEmpty(userAgent)
            || string.IsNullOrEmpty(ipAddress)
            || string.IsNullOrEmpty(hash)
            || string.IsNullOrEmpty(fingerprint))
        {
            throw new UrlAccessException("Some required parameters are missing in the request.");
        }

        var builder = new TrackerBuilder(userAgent)
                      .WithBrowser().WithOs().WithBrand().WithModel().WithIp(ipAddress).WithDevice();

        var tracker = builder.Build();

        context.HttpContext.Items["Tracker"] = tracker;
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
}