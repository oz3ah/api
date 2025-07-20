using Microsoft.AspNetCore.Mvc.Filters;
using Shortha.Domain;
using Shortha.Extenstions;

namespace Shortha.Filters;

public class TrackerFilter(ILogger<Serilog.ILogger> logger) : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        var query = context.HttpContext.Request.Query;
        var hash = query["hash"].ToString();
        var fingerprint = query["fingerprint"].ToString();
        var userAgent = context.HttpContext.Request.Headers["User-Agent"].ToString();
        var ipAddress = context.HttpContext.Request.Headers["CF-Connecting-IP"].FirstOrDefault()
                        ??
                        context.HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();
        var userId = context.HttpContext.User.GetUserIdOrNull();
        logger.LogInformation("TrackerFilter: UserAgent: {UserAgent}, IP: {IpAddress}, Hash: {Hash}, Fingerprint: {Fingerprint} User ID:{userID}",
                              userAgent, ipAddress, hash, fingerprint, userId);


        context.HttpContext.Items["RequestInfo"] = new RequestInfo()
        {
            hash = hash,
            fingerprint = fingerprint,
            ipAddress = ipAddress,
            userAgent = userAgent,
            userId = userId
        };
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
}