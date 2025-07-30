using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shortha.Application.Services;
using Shortha.Extenstions;

namespace Shortha.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SubscriptionController(ISubscriptionService subscription) : Base
{
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> SubscribeToPackage(string packageId)
    {
        var userId = User.GetUserId();
        var result = await subscription.Subscribe(userId, packageId);
        
       return Success(result);
    }
}