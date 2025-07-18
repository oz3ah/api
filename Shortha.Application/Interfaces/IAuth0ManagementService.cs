using Shortha.Application.Dto.Responses.Auth0;

namespace Shortha.Application.Interfaces;

public interface IAuth0ManagementService
{
    Task<Auth0UserResponse> GetUserInfoAsync(string userId);
}