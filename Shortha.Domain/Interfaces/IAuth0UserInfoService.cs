using Shortha.Domain.Dto;

namespace Shortha.Domain.Interfaces;

public interface IAuth0UserInfoService
{
    Task<UserInfoDto?> GetUserInfoAsync();
}