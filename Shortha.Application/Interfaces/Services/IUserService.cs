using Shortha.Application.Dto.Responses.Url;
using Shortha.Domain.Entites;

namespace Shortha.Application.Services;

public interface IUserService
{
    Task<AppUser> CreateUserAsync(string token);
    Task<(AppUser, UserUrlStatsResponse)> GetUserById(string userId);
    Task<AppUser> ChangeSubscriptionType(string userId, bool isPremium);
    Task AlternateUserRole(string newRole, string userId);
}