using Microsoft.EntityFrameworkCore;
using Shortha.Domain.Entites;
using Shortha.Domain.Interfaces.Repositories;

namespace Shortha.Infrastructre.Repositories;

public class AppConnectionRepository(DbContext context)
    : GenericRepository<AppConnection>(context), IAppConnectionRepository
{
}