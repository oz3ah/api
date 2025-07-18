using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shortha.Application.Dto.Requests.Url;
using Shortha.Application.Services;
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


            var url = await urlService.CreateUrl(submittedUrl, userId: userId);
            return Success(url);
        }
    }
}
