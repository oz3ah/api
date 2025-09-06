using Google.Apis.Auth;
using Shortha.Application.Dto.Responses.Url;
using Shortha.Application.Exceptions;
using Shortha.Application.Interfaces;
using Shortha.Application.Interfaces.Services;
using Shortha.Domain.Entites;
using Shortha.Domain.Interfaces.Repositories;

namespace Shortha.Application.Services;

public class UserService(
    IUserRepository repository,
    IAnalyticsService analyticsService,
    IRoleRepository roleRepository,
    ITokenService tokenService)
    : IUserService
{
    public async Task<string> CreateUserAsync(string token)
    {
        var payload = await GoogleJsonWebSignature.ValidateAsync(token);


        if (payload == null)
        {
            throw new NotFoundException("User not found in Google");
        }

        // Check if the user already exists in the repository
        var existingUser = await repository.GetByIdAsync(payload.Subject);
        if (existingUser != null)
        {
            // Update the existing user with new information
            existingUser.LastLoginAt = DateTime.UtcNow;
            await repository.SaveAsync();

            return tokenService.GenerateToken(existingUser.Id, existingUser.Email,
                existingUser.IsPremium ? "PRO" : "FREE");
        }

        var newUser = new AppUser
        {
            Id = payload.Subject,

            Email = payload.Email,
            Name = payload.Name,
            Picture = payload.Picture,
            CreatedAt = DateTime.UtcNow,
            LastLoginAt = DateTime.UtcNow,
            Provider = "Google",
            IsPremium = false,
            Role = await GetRoleByName("FREE")
        };


        await repository.AddAsync(newUser);
        await repository.SaveAsync();


        return tokenService.GenerateToken(newUser.Id, newUser.Email, "FREE");
        ;
    }

    public async Task<(AppUser, UserUrlStatsResponse)> GetUserById(string userId)
    {
        var user = await repository.GetByIdAsync(userId);
        if (user == null)
        {
            throw new NotFoundException("User not found");
        }

        var stats = await analyticsService.GetUserStats(user.Id);
        return (user, stats);
    }

    public Task<AppUser> ChangeSubscriptionType(string userId, bool isPremium)
    {
        return repository.UpdateSubscriptionType(isPremium, userId);
    }

    public async Task AlternateUserRole(string newRole, string userId)
    {
        var user = await repository.GetByIdAsync(userId);
        if (user == null)
        {
            throw new NotFoundException("User not found");
        }

        var role = await GetRoleByName(newRole);
        user.Role = role;
        user.IsPremium = newRole == "PRO";
        await repository.SaveAsync();
    }

    public async Task<bool> IsUserPremium(string userId)
    {
        var user = await repository.GetByIdAsync(userId);
        if (user == null)
        {
            throw new NotFoundException("User not found");
        }

        return user.IsPremium;
    }

    private async Task<Role> GetRoleByName(string name)
    {
        var role = await roleRepository.GetAsync(r => r.Name == name);
        if (role == null)
        {
            throw new NotFoundException("Role not found");
        }

        return role;
    }
}