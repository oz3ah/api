using Shortha.Domain.Entites;

namespace Shortha.Domain.Interfaces.Repositories;

public interface IUserRepository : IGenericRepository<AppUser>
{
    Task<AppUser> UpdateSubscriptionType(bool isPremium, string userId);
}