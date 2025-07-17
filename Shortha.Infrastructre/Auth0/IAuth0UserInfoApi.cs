using Refit;
using Shortha.Domain.Dto;

namespace Shortha.Infrastructre.Auth0;

public interface IAuth0UserInfoApi
{
    [Get("/userinfo")]
    Task<UserInfoDto> GetUserInfoAsync([Header("Authorization")] string authorization);
}