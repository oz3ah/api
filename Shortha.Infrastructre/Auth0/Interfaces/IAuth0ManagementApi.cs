using Refit;
using Shortha.Application.Dto.Requests.Auth0;
using Shortha.Application.Dto.Responses;
using Shortha.Application.Dto.Responses.Auth0;

namespace Shortha.Infrastructre.Auth0.Interfaces;

public interface IAuth0ManagementApi
{
    [Post("/oauth/token")]
    Task<Auth0TokenResponse> GetTokenAsync([Body] Auth0TokenRequest request);

    [Get("/api/v2/users/{id}")]
    Task<Auth0UserResponse> GetUserByIdAsync(
        string id,
        [Header("Authorization")] string authorization);

}