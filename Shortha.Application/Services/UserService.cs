using Shortha.Application.Exceptions;
using Shortha.Application.Interfaces;
using Shortha.Domain.Entites;
using Shortha.Domain.Interfaces.Repositories;

namespace Shortha.Application.Services;

public class UserService(IUserRepository repository, IAuth0ManagementService auth0) : IUserService
{
    public async Task<AppUser> CreateUserAsync(string userId)
    {
        var user = await auth0.GetUserInfoAsync(userId);

        if (user == null)
        {
            throw new NotFoundException("User not found in Auth0");
        }

        // Check if the user already exists in the repository
        var existingUser = await repository.GetByIdAsync(userId);
        if (existingUser != null)
        {
            // Update the existing user with new information
            existingUser.LastLoginAt = user.LastLogin;
            await repository.SaveAsync();
            return existingUser;
        }

        // If the user does not exist, create a new AppUser instance
        var newUser = new AppUser
        {
            Id = user.UserId,
            Email = user.Email,
            Name = user.Name,
            Picture = user.Picture,
            CreatedAt = user.CreatedAt,
            LastLoginAt = DateTime.UtcNow, // Set the last login time to now
            Provider = user.Identities.First().Provider,
        };

        await repository.AddAsync(newUser);
        await repository.SaveAsync();
        return newUser;



    }
}

public interface IUserService
{
    Task<AppUser> CreateUserAsync(string token);
}