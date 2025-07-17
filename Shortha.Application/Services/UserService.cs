using Shortha.Domain.Dto;
using Shortha.Domain.Entites;
using Shortha.Domain.Interfaces;
using Shortha.Domain.Interfaces.Repositories;

namespace Shortha.Application.Services;

public class UserService(IUserRepository repository, IAuth0UserInfoService auth0) : IUserService
{
    public async Task<UserInfoDto?> CreateUserAsync(string token)
    {
        var user = await auth0.GetUserInfoAsync(token);
        return user;
        // await repository.AddAsync(user);
        // await repository.SaveAsync();
        // return user;



    }
}

public interface IUserService
{
    Task<UserInfoDto?> CreateUserAsync(string token);
}