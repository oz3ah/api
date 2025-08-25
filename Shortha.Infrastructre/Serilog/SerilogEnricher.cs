using Microsoft.AspNetCore.Http;
using Serilog.Core;
using Serilog.Events;

namespace Shortha.Infrastructre.Serilog;

public class SerilogEnricher(
    string applicationId,
    string applicationName,
    string environmentName,
    IHttpContextAccessor? httpContextAccessor = null)
    : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        // Basic application properties
        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("ApplicationId", applicationId));
        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("ApplicationName", applicationName));
        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("Environment", environmentName));

        var httpContext = httpContextAccessor?.HttpContext;
        if (httpContext != null)
        {
            // Correlation ID for request tracking
            var correlationId = httpContext.TraceIdentifier;
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("CorrelationId", correlationId));

            // Request information
            var request = httpContext.Request;
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("RequestPath", request.Path.Value));
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("RequestMethod", request.Method));
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("RequestQuery", request.QueryString.Value));


            // IP Address and User Agent
            var ipAddress = httpContext.Request.Headers["X-Client-IP"].FirstOrDefault()
                            ?? httpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();
            var userAgent = httpContext.Request.Headers["User-Agent"].ToString();

            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("ClientIpAddress", ipAddress));
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("UserAgent", userAgent));

            // Response status code if available
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("ResponseStatusCode",
                                                                        httpContext.Response.StatusCode));
        }
    }
}