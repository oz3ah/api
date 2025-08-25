namespace Shortha.Domain.Dto;

public class ErrorResponse
{
    public bool Success { get; set; } = false;
    public string Message { get; set; } = "An error occurred.";
    public List<string>? Errors { get; set; }
    public string? TraceId { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string? Path { get; set; }
    public int? StatusCode { get; set; }
    public Dictionary<string, object>? AdditionalInfo { get; set; }

    public static ErrorResponse From(string message, List<string>? errors = null, string? traceId = null,
        string? path = null, int? statusCode = null, Dictionary<string, object>? additionalInfo = null)
        => new()
        {
            Message = message,
            Errors = errors,
            TraceId = traceId,
            Path = path,
            StatusCode = statusCode,
            AdditionalInfo = additionalInfo
        };

    public static ErrorResponse FromException(Exception ex, string? traceId = null,
        string? path = null, int? statusCode = null, bool includeStackTrace = false)
    {
        var additionalInfo = new Dictionary<string, object>
        {
            ["ExceptionType"] = ex.GetType().Name,
            ["Source"] = ex.Source ?? "Unknown"
        };

        if (includeStackTrace && !string.IsNullOrEmpty(ex.StackTrace))
        {
            additionalInfo["StackTrace"] = ex.StackTrace;
        }

        if (ex.InnerException != null)
        {
            additionalInfo["InnerException"] = new
            {
                Type = ex.InnerException.GetType().Name,
                Message = ex.InnerException.Message
            };
        }

        return new ErrorResponse
        {
            Message = ex.Message,
            TraceId = traceId,
            Path = path,
            StatusCode = statusCode,
            AdditionalInfo = additionalInfo
        };
    }
}