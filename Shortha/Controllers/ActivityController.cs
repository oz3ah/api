using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shortha.Application.Interfaces.Services;
using Shortha.Extenstions;

namespace Shortha.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ActivityController(IActivityService service) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetActivityByUserId([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var userId = User.GetUserId();
            var result = await service.GetAllActivities(userId, page, pageSize);
            return Ok(result);
        }
    }
}