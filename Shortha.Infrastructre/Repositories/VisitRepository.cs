using Shortha.Domain.Entites;
using Shortha.Domain.Interfaces.Repositories;

namespace Shortha.Infrastructre.Repositories
{
    public class VisitRepository(AppDb context) : GenericRepository<Visit>(context), IVisitRepository
    {





    }
}