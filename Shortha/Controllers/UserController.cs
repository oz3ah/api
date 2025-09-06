using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shortha.Application.Interfaces.Services;
using Shortha.Application.Services;
using Shortha.Extenstions;

namespace Shortha.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController(IUserService userService) : Base
    {
        [HttpGet("sync")]
        public async Task<IActionResult> Sync([FromQuery] string token)
        {
            var user = await userService.CreateUserAsync(token);

            return Success(user);
        }

        [Authorize]
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserById(string userId)
        {
            var tokenUserId = User.GetUserId();
            if (userId != tokenUserId)
            {
                return Fail("You can only access your own user data.");
            }

            var user = await userService.GetUserById(userId);
            return Success(new
            {
                user = user.Item1,
                stats = user.Item2
            });
        }

        [Authorize]
        [HttpGet("subscription-status")]
        public async Task<IActionResult> GetSubscriptionStatus()
        {
            var userId = User.GetUserId();
            var status = await userService.IsUserPremium(userId);
            return Success(status);
        }
    }
}