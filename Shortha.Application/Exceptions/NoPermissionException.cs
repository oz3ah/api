namespace Shortha.Application.Exceptions;

public class NoPermissionException : Exception
{
    public NoPermissionException()
        : base("You do not have permission to perform this action.")
    {
    }

    public NoPermissionException(string message)
        : base(message)
    {
    }
}