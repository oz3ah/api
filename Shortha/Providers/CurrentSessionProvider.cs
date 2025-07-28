using Shortha.Domain.Interfaces;
using System.Security.Claims;

namespace Shortha.Providers;

public class CurrentSessionProvider : ICurrentSessionProvider
{
    private readonly string? _currentUserId;

    public CurrentSessionProvider(IHttpContextAccessor accessor)
    {
        var userId = accessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
        {
            return;
        }

        _currentUserId = userId;
    }

    public string? GetUserId()
    {
        return _currentUserId;
    }
}