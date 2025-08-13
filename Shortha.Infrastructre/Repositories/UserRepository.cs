using Shortha.Application.Exceptions;
using Shortha.Domain.Entites;
using Shortha.Domain.Interfaces.Repositories;

namespace Shortha.Infrastructre.Repositories
{
    public class UserRepository(AppDb context) : GenericRepository<AppUser>(context), IUserRepository
    {
        public async Task<AppUser> UpdateSubscriptionType(bool isPremium, string userId)
        {
            var user = await GetByIdAsync(userId);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }

            user.IsPremium = isPremium;
            Update(user);
            await SaveAsync();
            return user;
        }
    }
}