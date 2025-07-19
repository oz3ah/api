namespace Shortha.Application.Exceptions;

public class UrlAccessException : Exception
{
    public UrlAccessException(string message) : base(message)
    {
    }

    public UrlAccessException(string message, Exception innerException) : base(message, innerException)
    {
    }
    
}