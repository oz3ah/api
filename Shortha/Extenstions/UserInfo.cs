using System.Security.Claims;

namespace Shortha.Extenstions;

public static class UserInfo
{
    public static string GetUserId(this ClaimsPrincipal user)
    {
        var id = user.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(id))
        {
            throw new UnauthorizedAccessException("No");
        }

        return id;
    }

    public static string? GetUserIdOrNull(this ClaimsPrincipal user)
    {
        return user.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}