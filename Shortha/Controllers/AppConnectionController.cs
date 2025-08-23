using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shortha.Application.Dto.Requests.AppConnections;
using Shortha.Application.Interfaces.Services;
using Shortha.Extenstions;

namespace Shortha.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppConnectionController(IAppConnectionService service) : Base
    {
        [HttpPost]
        public async Task<IActionResult> CreateConnection([FromBody] CreateConnectionDto connectionDto)
        {
            var result = await service.CreateNewConnection(connectionDto.Version, connectionDto.Device,
                connectionDto.DeviceMetadata);
            return Success(result);
        }

        [Authorize]
        [HttpPost("{pairCode:int}")]
        public async Task<IActionResult> ActivateConnection([FromRoute] int pairCode)
        {
            var userId = User.GetUserId();
            var result = await service.ActivateConnection(pairCode.ToString(), userId);
            return Success(result);
        }
    }
}