using System.Security.Claims;

namespace Shortha.Extenstions;

public static class ClaimsPrincipalExtensions
{
    public static bool IsPro(this ClaimsPrincipal user)
    {
        return user?.FindFirstValue(ClaimTypes.Role) == "PRO";
    }
}