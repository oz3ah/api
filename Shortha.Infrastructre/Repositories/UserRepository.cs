using Shortha.Domain.Entites;
using Shortha.Domain.Interfaces.Repositories;

namespace Shortha.Infrastructre.Repositories
{
    public class UserRepository(AppDb context) : GenericRepository<AppUser>(context), IUserRepository
    {
    }
}