using Microsoft.EntityFrameworkCore;
using Shortha.Domain.Entites;
using Shortha.Domain.Interfaces.Repositories;

namespace Shortha.Infrastructre.Repositories
{
    public class UserRepository(AppDb context) : IUserRepository
    {
        public async Task<AppUser> Create(AppUser user)
        {
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            return user;
        }

        public async Task<AppUser?> GetById(string userId)
        {
            return await context.Users
                                .Where(u => u.Id == userId)
                                .FirstOrDefaultAsync();
        }

        public async Task<AppUser> Update(AppUser user)
        {
            context.Users.Update(user);
            await context.SaveChangesAsync();
            return user;
        }
    }
}