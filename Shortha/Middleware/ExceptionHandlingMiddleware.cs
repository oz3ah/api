using Shortha.Application.Exceptions;
using Shortha.Domain.Dto;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Shortha.Middleware;

public class ExceptionHandlingMiddleware(
    RequestDelegate next,
    ILogger<ExceptionHandlingMiddleware> logger,
    IHostEnvironment env)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            // Capture comprehensive error information
            var errorContext = await CaptureErrorContextAsync(context, ex);

            ErrorResponse error;
            int statusCode;

            switch (ex)
            {
                case NotFoundException:
                    statusCode = (int)HttpStatusCode.NotFound;
                    error = ErrorResponse.From(ex.Message, traceId: context.TraceIdentifier,
                        path: context.Request.Path.Value, statusCode: statusCode);
                    logger.LogError(ex,
                        "NotFoundException occurred | CorrelationId: {CorrelationId} | " +
                        "Path: {Path} | Method: {Method} | UserId: {UserId} | " +
                        "RequestBody: {RequestBody} | Headers: {@Headers} | " +
                        "Message: {Message}",
                        errorContext.CorrelationId, errorContext.RequestPath, errorContext.RequestMethod,
                        errorContext.UserId, errorContext.RequestBody, errorContext.RequestHeaders,
                        ex.Message);
                    break;

                case ValidationException validationEx:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    error = ErrorResponse.From(validationEx.Message, validationEx.Errors, context.TraceIdentifier,
                        context.Request.Path.Value, statusCode);
                    logger.LogError(ex,
                        "ValidationException occurred | CorrelationId: {CorrelationId} | " +
                        "Path: {Path} | Method: {Method} | UserId: {UserId} | " +
                        "RequestBody: {RequestBody} | Headers: {@Headers} | " +
                        "ValidationErrors: {@ValidationErrors} | Message: {Message}",
                        errorContext.CorrelationId, errorContext.RequestPath, errorContext.RequestMethod,
                        errorContext.UserId, errorContext.RequestBody, errorContext.RequestHeaders,
                        validationEx.Errors, ex.Message);
                    break;

                case ConflictException:
                    statusCode = (int)HttpStatusCode.Conflict;
                    error = ErrorResponse.From(ex.Message, traceId: context.TraceIdentifier,
                        path: context.Request.Path.Value, statusCode: statusCode);
                    logger.LogError(ex,
                        "ConflictException occurred | CorrelationId: {CorrelationId} | " +
                        "Path: {Path} | Method: {Method} | UserId: {UserId} | " +
                        "RequestBody: {RequestBody} | Headers: {@Headers} | " +
                        "Message: {Message}",
                        errorContext.CorrelationId, errorContext.RequestPath, errorContext.RequestMethod,
                        errorContext.UserId, errorContext.RequestBody, errorContext.RequestHeaders,
                        ex.Message);
                    break;

                case NoPermissionException:
                    statusCode = (int)HttpStatusCode.Unauthorized;
                    error = ErrorResponse.From(ex.Message, traceId: context.TraceIdentifier,
                        path: context.Request.Path.Value, statusCode: statusCode);
                    logger.LogError(ex,
                        "NoPermissionException occurred | CorrelationId: {CorrelationId} | " +
                        "Path: {Path} | Method: {Method} | UserId: {UserId} | " +
                        "RequestBody: {RequestBody} | Headers: {@Headers} | " +
                        "Message: {Message}",
                        errorContext.CorrelationId, errorContext.RequestPath, errorContext.RequestMethod,
                        errorContext.UserId, errorContext.RequestBody, errorContext.RequestHeaders,
                        ex.Message);
                    break;

                case BadHttpRequestException:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    error = ErrorResponse.From(ex.Message, traceId: context.TraceIdentifier,
                        path: context.Request.Path.Value,
                        statusCode: statusCode);
                    logger.LogError(ex,
                        "NoPermissionException occurred | CorrelationId: {CorrelationId} | " +
                        "Path: {Path} | Method: {Method} | UserId: {UserId} | " +
                        "RequestBody: {RequestBody} | Headers: {@Headers} | " +
                        "Message: {Message}",
                        errorContext.CorrelationId, errorContext.RequestPath, errorContext.RequestMethod,
                        errorContext.UserId, errorContext.RequestBody, errorContext.RequestHeaders,
                        ex.Message);
                    break;

                default:
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    var message = env.IsDevelopment() ? ex.Message : "Internal Server Error";
                    var stack = env.IsDevelopment() ? new List<string> { ex.StackTrace ?? "" } : null;
                    error = ErrorResponse.From(message, stack, context.TraceIdentifier,
                        path: context.Request.Path.Value, statusCode: statusCode);

                    // Log comprehensive error information for internal server errors
                    logger.LogCritical(ex,
                        "Unhandled exception occurred | CorrelationId: {CorrelationId} | " +
                        "Path: {Path} | Method: {Method} | UserId: {UserId} | " +
                        "RequestBody: {RequestBody} | Headers: {@Headers} | " +
                        "QueryString: {QueryString} | UserAgent: {UserAgent} | " +
                        "ClientIpAddress: {ClientIpAddress} | ExceptionType: {ExceptionType} | " +
                        "Message: {Message} | StackTrace: {StackTrace} | " +
                        "InnerException: {InnerException} | Source: {Source} | " +
                        "TargetSite: {TargetSite} | Data: {@ExceptionData}",
                        errorContext.CorrelationId, errorContext.RequestPath, errorContext.RequestMethod,
                        errorContext.UserId, errorContext.RequestBody, errorContext.RequestHeaders,
                        errorContext.QueryString, errorContext.UserAgent, errorContext.ClientIpAddress,
                        ex.GetType().FullName, ex.Message, ex.StackTrace,
                        ex.InnerException?.ToString(), ex.Source, ex.TargetSite?.ToString(),
                        ex.Data);
                    break;
            }

            response.StatusCode = statusCode;
            await response.WriteAsJsonAsync(error);
        }
    }

    private async Task<ErrorContext> CaptureErrorContextAsync(HttpContext context, Exception exception)
    {
        var errorContext = new ErrorContext
        {
            CorrelationId = context.TraceIdentifier,
            RequestPath = context.Request.Path.Value ?? "",
            RequestMethod = context.Request.Method,
            QueryString = context.Request.QueryString.Value ?? "",
            UserAgent = context.Request.Headers["User-Agent"].ToString(),
            ClientIpAddress = context.Request.Headers["X-Client-IP"].FirstOrDefault()
                              ?? context.Connection.RemoteIpAddress?.MapToIPv4().ToString(),
            RequestHeaders = SanitizeHeaders(context.Request.Headers),
            UserId = context.User?.Identity?.IsAuthenticated == true
                ? context.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                : null,
            ExceptionOccurredAt = DateTime.UtcNow
        };

        // Capture request body if possible
        try
        {
            errorContext.RequestBody = await CaptureRequestBodyAsync(context.Request);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to capture request body for error context");
            errorContext.RequestBody = "[Failed to capture request body]";
        }

        return errorContext;
    }

    private async Task<string> CaptureRequestBodyAsync(HttpRequest request)
    {
        if (request.ContentLength == 0 || request.ContentLength == null)
            return "";

        // Enable buffering to allow multiple reads
        request.EnableBuffering();

        using var reader = new StreamReader(
            request.Body,
            encoding: Encoding.UTF8,
            detectEncodingFromByteOrderMarks: false,
            bufferSize: 1024,
            leaveOpen: true);

        var body = await reader.ReadToEndAsync();

        // Reset the request body stream position
        request.Body.Position = 0;

        return SanitizeRequestBody(body);
    }

    private string SanitizeRequestBody(string body)
    {
        if (string.IsNullOrWhiteSpace(body))
            return body;

        try
        {
            using var document = JsonDocument.Parse(body);
            var sanitized = SanitizeJsonObject(document.RootElement);
            return JsonSerializer.Serialize(sanitized, new JsonSerializerOptions { WriteIndented = false });
        }
        catch
        {
            // If not JSON, just return as is (could add more sanitization rules here)
            return body.Length > 1000 ? body.Substring(0, 1000) + "..." : body;
        }
    }

    private object SanitizeJsonObject(JsonElement element)
    {
        var sensitiveFields = new[] { "password", "token", "secret", "key", "authorization" };

        switch (element.ValueKind)
        {
            case JsonValueKind.Object:
                var obj = new Dictionary<string, object>();
                foreach (var property in element.EnumerateObject())
                {
                    var isSensitive = sensitiveFields.Any(field =>
                        property.Name.Contains(field,
                            StringComparison.OrdinalIgnoreCase));

                    obj[property.Name] = isSensitive ? "[REDACTED]" : SanitizeJsonObject(property.Value);
                }

                return obj;

            case JsonValueKind.Array:
                return element.EnumerateArray().Select(SanitizeJsonObject).ToArray();

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
        var sensitiveHeaders = new[] { "authorization", "x-api-key", "cookie" };
        var sanitized = new Dictionary<string, string>();

        foreach (var header in headers)
        {
            var isSensitive = sensitiveHeaders.Any(sensitive =>
                header.Key.Contains(sensitive,
                    StringComparison.OrdinalIgnoreCase));

            sanitized[header.Key] = isSensitive ? "[REDACTED]" : string.Join(", ", header.Value);
        }

        return sanitized;
    }

    private class ErrorContext
    {
        public string CorrelationId { get; set; } = "";
        public string RequestPath { get; set; } = "";
        public string RequestMethod { get; set; } = "";
        public string QueryString { get; set; } = "";
        public string UserAgent { get; set; } = "";
        public string? ClientIpAddress { get; set; }
        public string RequestBody { get; set; } = "";
        public Dictionary<string, string> RequestHeaders { get; set; } = new();
        public string? UserId { get; set; }
        public DateTime ExceptionOccurredAt { get; set; }
    }
}