using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shortha.Application.Dto.Requests.Url;
using Shortha.Application.Services;
using System.Security.Claims;
using Shortha.Domain;
using Shortha.Extenstions;
using Shortha.Filters;

namespace Shortha.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UrlController(IUrlService urlService) : Base
    {
        [HttpPost("create")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateNew([FromBody] UrlCreateRequest submittedUrl)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            bool isPro = false;

            if (!string.IsNullOrEmpty(userId))
            {
                var permissions = User.GetPermissions();

                isPro = permissions.Contains("create:expire") && permissions.Contains("create:custom");
            }

            var url = await urlService.CreateUrl(submittedUrl, userId, isPro);
            return Success(url);
        }

        [HttpGet("my-links")]
        [Authorize]
        public async Task<IActionResult> GetMyLinks()
        {
            var userId = User.GetUserId();
        
            var urls = await urlService.GetUrlsByUserId(userId);
            return Ok(urls);
        }

        [HttpGet]
        [AllowAnonymous]
        [ServiceFilter(typeof(TrackerFilter))]
        public async Task<IActionResult> OpenUrl([FromQuery] GetUrlFromCodeRequest submittedHash)
        {
            var tracker = HttpContext.Items["Tracker"] as Tracker;
            // var url = await urlService.OpenUrl(submittedHash.Hash, tracker);

            return Ok(tracker);
        }

    }
}