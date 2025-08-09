using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shortha.Application.Dto.Requests.Url;
using Shortha.Application.Interfaces.Services;
using Shortha.Extenstions;
using Shortha.Filters;
using System.Security.Claims;

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

        [HttpGet("deactivate/{id}")]
        [Authorize]
        public async Task<IActionResult> DeactivateUrl(string id)
        {
            var userId = User.GetUserId();

            var url = await urlService.DeactivateUrl(id, userId);
            return Success(url);
        }
        
        [HttpGet("activate/{id}")]
        [Authorize]
        public async Task<IActionResult> ActivateUrl(string id)
        {
            var userId = User.GetUserId();

            var url = await urlService.ActivateUrl(id, userId);
            return Success(url);
        }

        [HttpGet("my-links")]
        [Authorize]
        public async Task<IActionResult> GetMyLinks([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var userId = User.GetUserId();

            var urls = await urlService.GetUrlsByUserId(userId, page, pageSize);
            return Success(urls);
        }

        [HttpGet]
        [AllowAnonymous]
        [ServiceFilter(typeof(TrackerFilter))]
        public async Task<IActionResult> OpenUrl([FromQuery] GetUrlFromCodeRequest submittedHash)
        {
            var request = HttpContext.GetRequestInfo();
            var userId = User.GetUserIdOrNull();
            var url = await urlService.OpenUrl(submittedHash.Hash, request);

            return Success(url);
        }
    }
}