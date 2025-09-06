using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shortha.Application.Dto.Requests.Api;
using Shortha.Application.Exceptions;
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
            var isPro = User.IsPro();

            if (!isPro)
            {
                throw new NoPermissionException("You need a Pro Subscription to be able to use api keys");
            }

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
            var result = await apiKeyService.GetUserKeys(User.GetUserId(), page, pageSize);
            return Success(result);
        }

        [HttpDelete("{keyId}")]
        [Authorize]
        public async Task<IActionResult> RevokeKey(string keyId)
        {
            await apiKeyService.Revoke(keyId);
            return Success("API key revoked successfully.");
        }
    }
}