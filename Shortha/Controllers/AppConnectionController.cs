using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shortha.Application.Dto.Requests.AppConnections;
using Shortha.Application.Interfaces.Services;

namespace Shortha.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppConnectionController(IAppConnectionService service) : Base
    {
        [HttpPost]
        public async Task<IActionResult> CreateConnection([FromBody] CreateConnectionDto connectionDto)
        {
            var result = await service.CreateNewConnection(connectionDto.Version, connectionDto.Device);
            return Success(result);
        }
    }
}