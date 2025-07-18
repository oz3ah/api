namespace Shortha.Domain.Dto;

public class ErrorResponse
{
    public bool Success { get; set; } = false;
    public string Message { get; set; } = "An error occurred.";
    public List<string>? Errors { get; set; }
    public string? TraceId { get; set; }

    public static ErrorResponse From(string message, List<string>? errors = null, string? traceId = null)
        => new() { Message = message, Errors = errors, TraceId = traceId };
}