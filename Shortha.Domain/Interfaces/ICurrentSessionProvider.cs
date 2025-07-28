namespace Shortha.Domain.Interfaces;

public interface ICurrentSessionProvider
{
    string? GetUserId();
}