using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Shortha.Domain.Entites;
using Shortha.Domain.Interfaces.Repositories;

namespace Shortha.Infrastructre.Repositories
{
    public class VisitRepository(AppDb context) : IVisitRepository
    {
        public async Task<Visit> CreateVisit(Visit visit)
        {
            await context.Visits.AddAsync(visit);
            await context.SaveChangesAsync();

            return visit;
        }


        public Visit? FindBy(Expression<Func<Visit, bool>> predicate)
        {
            return context.Visits
                          .AsNoTracking()
                          .FirstOrDefault(predicate);
        }

        public async Task<(IEnumerable<Visit>, int)> GetAllBy(Expression<Func<Visit, bool>>? predicate,
                                                              Expression<Func<Visit, object>>? orderBy = null,
                                                              bool ascending = true, int page = 1)
        {
            var query = context.Visits.AsNoTracking();
            var totalCount = await query.CountAsync();
            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                query = ascending
                    ? query.OrderBy(orderBy)
                    : query.OrderByDescending(orderBy);
            }

            return (await query
                          .Skip((page - 1) * 10)
                          .Take(10)
                          .ToListAsync(), totalCount);
        }
    }
}