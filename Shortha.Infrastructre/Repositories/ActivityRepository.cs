using Microsoft.EntityFrameworkCore;
using Shortha.Domain.Entites;
using Shortha.Domain.Interfaces;
using Shortha.Domain.Interfaces.Repositories;

namespace Shortha.Infrastructre.Repositories;

public class ActivityRepository(AppDb dbContext) : GenericRepository<AuditTrail>(dbContext), IActivityRepository
{
}