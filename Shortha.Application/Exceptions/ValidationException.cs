namespace Shortha.Application.Exceptions;

public class ValidationException(List<string> errors) : Exception("Validation Failed")
{
    public List<string> Errors { get; } = errors;
}