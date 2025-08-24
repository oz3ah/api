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
        [HttpPost("{pairCode}")]
        public async Task<IActionResult> ActivateConnection([FromRoute] string pairCode)
        {
            var userId = User.GetUserId();
            var result = await service.ActivateConnection(pairCode, userId);
            return Success(result);
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> RevokeConnection([FromQuery] string connectionId)
        {
            await service.RevokeConnection(connectionId, User.GetUserId());
            return Success<string>(null);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllConnections([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var result = await service.GetAllByUserId(User.GetUserId(), page, pageSize);
            return Success(result);
        }

        [HttpGet("status/{pairCode}")]
        public async Task<IActionResult> GetConnectionStatus([FromRoute] string pairCode)
        {
            var connection = await service.IsConnectedByPairCode(pairCode);
            return Success(connection);
        }
    }
}