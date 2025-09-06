using Shortha.Application.Dto.Responses.Url;
using Shortha.Domain.Entites;

namespace Shortha.Application.Interfaces.Services;

public interface IUserService
{
    Task<string> CreateUserAsync(string token);
    Task<(AppUser, UserUrlStatsResponse)> GetUserById(string userId);
    Task<AppUser> ChangeSubscriptionType(string userId, bool isPremium);
    Task AlternateUserRole(string newRole, string userId);
    Task<bool> IsUserPremium(string userId);
}