using System.Security.Claims;

namespace Shortha.Extenstions;

public static class UserInfo
{
    public static string GetUserId(this ClaimsPrincipal user)
    {
        var id =  user.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(id))
        {
            throw new UnauthorizedAccessException("User ID not found in claims.");
        }
        return id;
    }

   
}