using IPinfo;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.Filters;
using Shortha.Application.Exceptions;
using Shortha.Infrastructre;

namespace Shortha.Filters;

public class TrackerFilter(IPinfoClient client) : IActionFilter
{

    public void OnActionExecuting(ActionExecutingContext context)
    {
        var query = context.HttpContext.Request.Query;
        var hash = query["hash"].ToString();
        var fingerprint = query["fingerprint"].ToString();
        var userAgent = context.HttpContext.Request.Headers["User-Agent"].ToString();
        var ipAddress = context.HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress?.ToString();

        if (string.IsNullOrEmpty(userAgent)
            || string.IsNullOrEmpty(ipAddress)
            || string.IsNullOrEmpty(hash)
            || string.IsNullOrEmpty(fingerprint))
        {
            throw new UrlAccessException("Some required parameters are missing in the request.");
        }

        var builder = new TrackerBuilder(userAgent, client)
                      .WithBrowser().WithOs().WithBrand().WithModel().WithIpAddress(ipAddress).WithIsBot()
                      .WithTimeZone();

        var tracker = builder.Build();

        context.HttpContext.Items["Tracker"] = tracker;
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
}