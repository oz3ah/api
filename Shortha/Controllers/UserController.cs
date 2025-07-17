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


            var userInfo = new UserInfoDto
                           {
                               Email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value,
                                 Name = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value,
                                 
                           };
                
                return Ok(userInfo);
        }
    }
}