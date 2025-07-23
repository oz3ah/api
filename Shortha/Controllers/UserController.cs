using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shortha.Application.Services;
using Shortha.Extenstions;

namespace Shortha.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController(IUserService userService) : Base
    {
        [Authorize]
        [HttpGet("sync")]
        public async Task<IActionResult> Sync()
        {
            var userId = User.GetUserId();

            var user = await userService.CreateUserAsync(userId);

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
            return Success(user);
        }
    }
}