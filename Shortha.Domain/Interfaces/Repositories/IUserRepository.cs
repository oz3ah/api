using Shortha.Domain.Entites;

namespace Shortha.Domain.Interfaces.Repositories;

public interface IUserRepository
{
    Task<AppUser> Create(AppUser user);
    Task<AppUser?> GetById(string userId);
    Task<AppUser> Update(AppUser user);
}