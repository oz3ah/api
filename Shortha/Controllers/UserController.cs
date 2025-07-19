using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Shortha.Application.Services;
using Shortha.Domain.Dto;

namespace Shortha.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController(IUserService userService) :  Base
    {
      
        [Authorize]
        [HttpGet("sync")]
        public async Task<IActionResult> Sync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await    userService.CreateUserAsync(userId);
                
                return Success(user);
        }
    }
}