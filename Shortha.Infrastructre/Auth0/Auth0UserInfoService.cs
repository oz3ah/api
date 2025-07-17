using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Shortha.Domain.Dto;
using Shortha.Domain.Interfaces;

namespace Shortha.Infrastructre.Auth0;

public class Auth0UserInfoService(IAuth0UserInfoApi api) : IAuth0UserInfoService
{
    public async Task<UserInfoDto?> GetUserInfoAsync(string token)
    {
      
            return await api.GetUserInfoAsync($"Bearer {token}");
    }
}