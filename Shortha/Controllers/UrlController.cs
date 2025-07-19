using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shortha.Application.Dto.Requests.Url;
using Shortha.Application.Services;
using System.Security.Claims;
using Shortha.Extenstions;

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

        // [HttpGet("my-links")]
        // [Authorize]
        // public async Task<IActionResult> GetMyLinks()
        // {
        //     var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //
        //     var urls = await urlService.GetUrlsByUserId(userId);
        //     return Ok(urls);
        // }


    }
}