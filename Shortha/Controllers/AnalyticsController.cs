using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shortha.Application.Services;
using Shortha.Extenstions;

namespace Shortha.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AnalyticsController(ILogger<AnalyticsController> logger, IAnalyticsService analytics) : Base
    {
        [HttpGet]
        public async Task<IActionResult> BasicAnalytics()
        {
            var userId = User.GetUserId();
            var result = await analytics.GetUserStats(userId);

            return Success(result);
        }
    }
}