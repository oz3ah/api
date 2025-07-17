using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Shortha.Application.Services;
using Shortha.Domain.Dto;

namespace Shortha.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController(IUserService userService) : ControllerBase
    {
      
        [Authorize]
        [HttpGet("sync")]
        public async Task<IActionResult> Sync()
        {
            
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

   var user =      await    userService.CreateUserAsync(token);
                
                return Ok(user);
        }
    }
}