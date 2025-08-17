using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shortha.Application.Dto.Requests.Api;
using Shortha.Application.Services;
using Shortha.Extenstions;

namespace Shortha.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiController(IApiKeyService apiKeyService) : Base
    {
        [HttpPost("new-key")]
        [Authorize]
        public async Task<IActionResult> CreateNewKey([FromBody] CreateApiKeyDto apiKeyDto)
        {
            var result = await apiKeyService.GenerateApiKeyByUserId(
                apiKeyDto.KeyName,
                User.GetUserId(),
                apiKeyDto.ExpirationDate);
            return Success(result);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserKeys([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var result = await apiKeyService.GetUserKeys(User.GetUserId());
            return Success(result);
        }
    }
}