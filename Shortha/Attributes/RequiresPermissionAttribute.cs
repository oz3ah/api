using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;
using Shortha.Application.Exceptions;
using Shortha.Domain.Enums;
using Shortha.Extenstions;

namespace Shortha.Attributes;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class RequiresPermissionAttribute(PermissionMode mode, params string[] permissions) : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var user = context.HttpContext.User;
        var userId = user.GetUserIdOrNull();

        if (userId != null)
        {
            var userPermissions = user.GetPermissions();

            switch (mode)
            {
                case PermissionMode.RequireAll:
                {
                    bool hasAllPermissions = permissions.All(p => userPermissions.Contains(p));
                    if (!hasAllPermissions)
                    {
                        throw new NoPermissionException("This action requires a Pro account");
                    }

                    break;
                }
                case PermissionMode.Optional:
                {
                    var isOneOfThem = permissions.Any(p => userPermissions.Contains(p));

                    if (!isOneOfThem)
                    {
                        Log.Warning(
                            "User {UserId} attempted to access a feature requiring one of the following permissions: {Permissions}",
                            userId, string.Join(", ", permissions));
                        throw new NoPermissionException("This action requires a Pro account");
                    }


                    break;
                }
            }
        }

        base.OnActionExecuting(context);
    }
}