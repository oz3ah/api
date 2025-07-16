using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Shortha.Domain.Services;

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
        
            
            return Ok(new
                      {
                          user = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value,
                      });
        }
    }
}