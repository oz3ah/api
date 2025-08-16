using Shortha.Domain.Entites;
using Shortha.Domain.Interfaces.Repositories;

namespace Shortha.Infrastructre.Repositories
{
    public class ApiRepository(AppDb context) : GenericRepository<Api>(context), IApiRepository
    {
    }
}