using Microsoft.EntityFrameworkCore;
using Shortha.Domain.Entites;
using Shortha.Domain.Interfaces.Repositories;

namespace Shortha.Infrastructre.Repositories;

public class RoleRepository(AppDb context) : GenericRepository<Role>(context), IRoleRepository
{
}