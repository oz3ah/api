using Shortha.Domain.Entites;
using Shortha.Domain.Interfaces.Repositories;

namespace Shortha.Domain.Services;

public class UserService(IUserRepository repository) : IUserService
{
    public async Task<AppUser> CreateUserAsync(AppUser user)
    {
        
        await repository.AddAsync(user);
        await repository.SaveAsync();
        return user;
        
        
        
    }
}

public interface IUserService
{
    Task<AppUser> CreateUserAsync(AppUser user);
}