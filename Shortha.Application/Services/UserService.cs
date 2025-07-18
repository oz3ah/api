using Shortha.Application.Dto.Responses.Auth0;
using Shortha.Application.Interfaces;
using Shortha.Domain.Interfaces.Repositories;

namespace Shortha.Application.Services;

public class UserService(IUserRepository repository, IAuth0ManagementService auth0) : IUserService
{
    public async Task<Auth0UserResponse> CreateUserAsync(string userId)
    {
        var user = await auth0.GetUserInfoAsync(userId);
        
        if (user == null)
        {
            throw new Exception("User not found in Auth0");
        }
        return user;
        // await repository.AddAsync(user);
        // await repository.SaveAsync();
        // return user;



    }
}

public interface IUserService
{
    Task<Auth0UserResponse> CreateUserAsync(string token);
}