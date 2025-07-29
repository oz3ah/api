using Shortha.Domain.Entites;
using Shortha.Domain.Interfaces.Repositories;

namespace Shortha.Infrastructre.Repositories
{
    public class PackageRepository(AppDb context) : GenericRepository<Package>(context), IPackageRepository
    {
    }
}