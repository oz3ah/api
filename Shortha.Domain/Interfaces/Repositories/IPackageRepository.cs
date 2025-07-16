using Shortha.Domain.Enums;
using System.Linq.Expressions;
using Shortha.Domain.Entites;

namespace Shortha.Domain.Interfaces.Repositories
{
    public interface IPackageRepository
    {
        Task<IReadOnlyList<Package>> GetPackages(Expression<Func<Package, bool>>? whereExpression = null);
        Task<Package?> GetPackageById(string packageId, Expression<Func<Package, bool>>? whereExpression = null);
        Task<Package?> GetPackageByName(PackagesNames packageName);
        Task<Package> CreatePackage(Package package);
        Task<Package> UpdatePackage(Package package);
        Task<Package> DeletePackage(string packageId);
    }
}