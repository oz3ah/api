using Shortha.Application.Dto.Requests.Auth0;
using Shortha.Application.Dto.Responses.Auth0;
using Shortha.Application.Exceptions;
using Shortha.Application.Interfaces;
using Shortha.Domain.Interfaces;
using Shortha.Infrastructre.Auth0.Interfaces;

namespace Shortha.Infrastructre.Auth0;

public class Auth0ManagementService(IAuth0ManagementApi managementApi, ISecretService secretService)
    : IAuth0ManagementService
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

    public async Task AssignRoleToUser(string name, string userId)
    {
        var managementToken = await GetManagementApiTokenAsync();

        var roles = await managementApi.GetRolesAsync($"Bearer {managementToken}");

        var role = roles.FirstOrDefault(r => r.Name == name);
        if (role == null)
        {
            throw new NotFoundException($"Role with name {name} not found.");
        }

        var roleAssignRequest = new RoleAssignRequest
        {
            Roles = [role.Id]
        };
        await managementApi.GetUserRolesAsync(userId, $"Bearer {managementToken}", roleAssignRequest);
    }
}