using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace Shortha.Middleware;

public class RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
{
    private const int MaxLogBodyChars = 100_000; // ~100 KB

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        var correlationId = context.TraceIdentifier;

        // Log request
        await LogRequestAsync(context, correlationId);

        // Capture response
        var originalResponseBodyStream = context.Response.Body;
        using var responseBodyStream = new MemoryStream();
        context.Response.Body = responseBodyStream;

        try
        {
            await next(context);
        }
        finally
        {
            stopwatch.Stop();

            // Log response
            await LogResponseAsync(context, correlationId, stopwatch.ElapsedMilliseconds, responseBodyStream);

            // Copy response back to original stream
            responseBodyStream.Seek(0, SeekOrigin.Begin);
            await responseBodyStream.CopyToAsync(originalResponseBodyStream);
        }
    }

    private async Task LogRequestAsync(HttpContext context, string correlationId)
    {
        var request = context.Request;

        try
        {
            var requestBody = await ReadRequestBodyAsync(request);
            var sanitizedBody = SanitizeRequestBody(requestBody);
            var sanitizedHeaders = SanitizeHeaders(request.Headers);

            logger.LogInformation(
                "HTTP Request: {Method} {Path}{Query} | CorrelationId: {CorrelationId} | " +
                "ContentType: {ContentType} | ContentLength: {ContentLength} | " +
                "Headers: {@Headers} | Body: {Body}",
                request.Method,
                request.Path,
                request.QueryString,
                correlationId,
                request.ContentType,
                request.ContentLength,
                sanitizedHeaders,
                sanitizedBody);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to log request details for CorrelationId: {CorrelationId}", correlationId);
        }
    }

    private async Task LogResponseAsync(HttpContext context, string correlationId, long elapsedMs,
        MemoryStream responseBodyStream)
    {
        var response = context.Response;

        try
        {
            var responseBody = await ReadResponseBodyAsync(responseBodyStream);
            var sanitizedHeaders = SanitizeHeaders(response.Headers);

            var logLevel = response.StatusCode >= 400 ? LogLevel.Warning : LogLevel.Information;

            logger.Log(logLevel,
                "HTTP Response: {StatusCode} | CorrelationId: {CorrelationId} | " +
                "Duration: {ElapsedMs}ms | ContentType: {ContentType} | " +
                "ContentLength: {ContentLength} | Headers: {@Headers} | Body: {Body}",
                response.StatusCode,
                correlationId,
                elapsedMs,
                response.ContentType,
                response.ContentLength,
                sanitizedHeaders,
                responseBody);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to log response details for CorrelationId: {CorrelationId}", correlationId);
        }
    }

    private async Task<string> ReadRequestBodyAsync(HttpRequest request)
    {
        if (request.ContentLength == 0 || request.ContentLength == null)
            return string.Empty;

        if (!IsTextContentType(request.ContentType))
            return $"[Binary content: {request.ContentType}]";

        request.EnableBuffering();
        using var reader = new StreamReader(request.Body, Encoding.UTF8, detectEncodingFromByteOrderMarks: false,
            bufferSize: 4096, leaveOpen: true);
        var content = await reader.ReadToEndAsync();
        request.Body.Position = 0;
        if (content.Length > MaxLogBodyChars)
            content = content[..MaxLogBodyChars] + "...[truncated]";
        return content;
    }

    private async Task<string> ReadResponseBodyAsync(MemoryStream responseBodyStream)
    {
        if (responseBodyStream.Length == 0)
            return string.Empty;

        responseBodyStream.Seek(0, SeekOrigin.Begin);
        using var reader = new StreamReader(responseBodyStream, Encoding.UTF8, leaveOpen: true);
        var content = await reader.ReadToEndAsync();
        responseBodyStream.Seek(0, SeekOrigin.Begin);

        return content;
    }

    private string SanitizeRequestBody(string body)
    {
        if (string.IsNullOrWhiteSpace(body))
            return body;

        try
        {
            using var document = JsonDocument.Parse(body);
            var sanitized = SanitizeJsonElement(document.RootElement);
            return JsonSerializer.Serialize(sanitized, new JsonSerializerOptions { WriteIndented = false });
        }
        catch
        {
            return body.Contains("password", StringComparison.OrdinalIgnoreCase) ? "[REDACTED]" : body;
        }
    }

    private object SanitizeJsonElement(JsonElement element)
    {
        switch (element.ValueKind)
        {
            case JsonValueKind.Object:
                var obj = new Dictionary<string, object>();
                foreach (var property in element.EnumerateObject())
                {
                    obj[property.Name] = SanitizeJsonElement(property.Value);
                }

                return obj;

            case JsonValueKind.Array:
                return element.EnumerateArray().Select(SanitizeJsonElement).ToArray();

            case JsonValueKind.String:
                return element.GetString();

            case JsonValueKind.Number:
                return element.GetDecimal();

            case JsonValueKind.True:
            case JsonValueKind.False:
                return element.GetBoolean();

            case JsonValueKind.Null:
                return null;

            default:
                return element.ToString();
        }
    }

    private Dictionary<string, string> SanitizeHeaders(IHeaderDictionary headers)
    {
        var sanitized = new Dictionary<string, string>();

        foreach (var header in headers)
        {
            sanitized[header.Key] = string.Join(", ", header.Value);
        }

        return sanitized;
    }


    private static bool IsTextContentType(string? contentType)
    {
        if (string.IsNullOrWhiteSpace(contentType))
            return false;

        var textTypes = new[] { "application/json", "application/xml", "text/", "application/x-www-form-urlencoded" };
        return textTypes.Any(type => contentType.StartsWith(type, StringComparison.OrdinalIgnoreCase));
    }
}