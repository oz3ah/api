using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shortha.Application.Interfaces.Services;
using Shortha.Application.Services;
using Shortha.Extenstions;

namespace Shortha.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VisitController(IVisitService service) : Base
    {
        [HttpGet("{shortUrl}")]
        [Authorize]
        public async Task<IActionResult> GetVisitsByShortUrl(string shortUrl, [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            if (!User.IsPro())
                Fail("You need a Pro account");

            var visits = await service.GetVisitsByShortUrl(shortUrl, page, pageSize);


            return Success(visits);
        }
    }
}