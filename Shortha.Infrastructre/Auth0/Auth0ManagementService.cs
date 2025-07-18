using Microsoft.Extensions.Configuration;
using Shortha.Application.Dto.Requests.Auth0;
using Shortha.Application.Dto.Responses.Auth0;
using Shortha.Application.Interfaces;
using Shortha.Domain.Dto;
using Shortha.Domain.Interfaces;
using Shortha.Infrastructre.Auth0.Interfaces;

namespace Shortha.Infrastructre.Auth0;

public class Auth0ManagementService(IAuth0ManagementApi managementApi, ISecretService secretService) : IAuth0ManagementService
{

  

    private async Task<string?> GetManagementApiTokenAsync()
    {
        var request = new Auth0TokenRequest()
                      {
                          ClientId = secretService.GetSecret("ClientId"),
                          ClientSecret = secretService.GetSecret("ClientSecret"),
                          Audience = secretService.GetSecret("ManagementAudience"),
                      };

        var response = await managementApi.GetTokenAsync(request);
        return response.AccessToken;
    }
    
    public async Task<Auth0UserResponse> GetUserInfoAsync(string userId)
    {
        var managementToken = await GetManagementApiTokenAsync();
      

        return await managementApi.GetUserByIdAsync(userId, $"Bearer {managementToken}");
    }
}