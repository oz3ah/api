using System.Diagnostics;

namespace Shortha.Middleware;

public class PerformanceMonitoringMiddleware(RequestDelegate next, ILogger<PerformanceMonitoringMiddleware> logger)
{
    private const int SlowRequestThresholdMs = 1000;
    private const int VerySlowRequestThresholdMs = 5000;

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        var correlationId = context.TraceIdentifier;
        var requestPath = context.Request.Path.Value;
        var requestMethod = context.Request.Method;

        try
        {
            await next(context);
        }
        finally
        {
            stopwatch.Stop();
            var elapsedMs = stopwatch.ElapsedMilliseconds;
            var statusCode = context.Response.StatusCode;

            // Log performance metrics
            LogPerformanceMetrics(correlationId, requestMethod, requestPath, statusCode, elapsedMs);
        }
    }

    private void LogPerformanceMetrics(string correlationId, string method, string? path, 
        int statusCode, long elapsedMs)
    {
        var logLevel = DetermineLogLevel(elapsedMs, statusCode);
        
        logger.Log(logLevel,
            "Request Performance | CorrelationId: {CorrelationId} | " +
            "{Method} {Path} | StatusCode: {StatusCode} | " +
            "Duration: {ElapsedMs}ms | Category: {PerformanceCategory}",
            correlationId, method, path, statusCode, elapsedMs, 
            GetPerformanceCategory(elapsedMs));

        // Log additional details for slow requests
        if (elapsedMs >= SlowRequestThresholdMs)
        {
            logger.LogWarning(
                "Slow Request Detected | CorrelationId: {CorrelationId} | " +
                "{Method} {Path} | Duration: {ElapsedMs}ms | " +
                "Threshold: {ThresholdMs}ms | StatusCode: {StatusCode}",
                correlationId, method, path, elapsedMs, SlowRequestThresholdMs, statusCode);
        }

        if (elapsedMs >= VerySlowRequestThresholdMs)
        {
            logger.LogError(
                "Very Slow Request Detected | CorrelationId: {CorrelationId} | " +
                "{Method} {Path} | Duration: {ElapsedMs}ms | " +
                "Threshold: {ThresholdMs}ms | StatusCode: {StatusCode} | " +
                "Action Required: Performance Investigation Needed",
                correlationId, method, path, elapsedMs, VerySlowRequestThresholdMs, statusCode);
        }
    }

    private static LogLevel DetermineLogLevel(long elapsedMs, int statusCode)
    {
        // Error status codes get warning level regardless of performance
        if (statusCode >= 400)
            return LogLevel.Warning;

        // Performance-based log levels
        return elapsedMs switch
        {
            >= VerySlowRequestThresholdMs => LogLevel.Error,
            >= SlowRequestThresholdMs => LogLevel.Warning,
            _ => LogLevel.Information
        };
    }

    private static string GetPerformanceCategory(long elapsedMs)
    {
        return elapsedMs switch
        {
            < 100 => "Fast",
            < 500 => "Normal",
            < SlowRequestThresholdMs => "Acceptable",
            < VerySlowRequestThresholdMs => "Slow",
            _ => "Very Slow"
        };
    }
}