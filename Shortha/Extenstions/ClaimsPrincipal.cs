using System.Security.Claims;

namespace Shortha.Extenstions;

public static class ClaimsPrincipalExtensions
{
    public static HashSet<string> GetPermissions(this ClaimsPrincipal user)
    {
        return user?.FindAll("permissions")
            ?.Select(p => p.Value)
            ?.ToHashSet() ?? [];
    }

    public static bool HasPermission(this ClaimsPrincipal user, string permission)
    {
        return user?.FindAll("permissions").Any(p => p.Value == permission) ?? false;
    }
}