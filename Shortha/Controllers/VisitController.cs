using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shortha.Application.Services;

namespace Shortha.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VisitController(IVisitService service) : Base
    {
        [HttpGet("{shortUrl}")]
        [Authorize]
        public async Task<IActionResult> GetVisitsByShortUrl([FromQuery] string shortUrl, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {

            var visits = await service.GetVisitsByShortUrl(shortUrl, page, pageSize);


            return Success(visits);
        }
    }
}
