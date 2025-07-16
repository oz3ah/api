using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Shortha.Domain.Entites;
using Shortha.Domain.Enums;
using Shortha.Domain.Interfaces.Repositories;

namespace Shortha.Infrastructre.Repositories
{
    public class PackageRepository(AppDb context) : IPackageRepository
    {
        public async Task<IReadOnlyList<Package>> GetPackages(Expression<Func<Package, bool>>? whereExpression = null)
        {
            var query = context.Packages.AsQueryable();
            if (whereExpression is not null)
                query = query.Where(whereExpression);
            return await query.ToListAsync();
        }

        public async Task<Package?> GetPackageById(string packageId,
                                                   Expression<Func<Package, bool>>? whereExpression = null)
        {
            var query = context.Packages.AsQueryable();
            if (whereExpression is not null)
                query = query.Where(whereExpression);
            return await query.FirstOrDefaultAsync(p => p.Id == packageId);
        }


        public async Task<Package?> GetPackageByName(PackagesName packageName)
        {
            return await context.Packages
                                .FirstOrDefaultAsync(p => p.Name == packageName);
        }

        public async Task<Package> CreatePackage(Package package)
        {
            await context.Packages.AddAsync(package);
            await context.SaveChangesAsync();

            return package;
        }

        public async Task<Package> UpdatePackage(Package package)
        {
            context.Packages.Update(package);


            await context.SaveChangesAsync();

            return package;
        }

        public async Task<Package?> DeletePackage(string packageId)
        {
            var package = await GetPackageById(packageId);

            if (package != null)
            {
                package.IsActive = false;
                context.Packages.Update(package);
                await context.SaveChangesAsync();
            }

            return package;
        }
    }
}