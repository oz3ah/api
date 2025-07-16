using System.Linq.Expressions;
using Shortha.Domain.Entites;

namespace Shortha.Domain.Interfaces.Repositories
{
    public interface IVisitRepository
    {
        public Visit? FindBy(
            Expression<Func<Visit, bool>> predicate
        );

        Task<(IEnumerable<Visit>, int)> GetAllBy(Expression<Func<Visit, bool>>? predicate,
                                                 Expression<Func<Visit, object>>? orderBy = null,
                                                 bool ascending = true, int page = 1);

        Task<Visit> CreateVisit(Visit visit);
    }
}