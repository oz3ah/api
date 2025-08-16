using AutoMapper;
using Shortha.Application.Dto.Responses.Package;
using Shortha.Application.Exceptions;
using Shortha.Domain.Dto;
using Shortha.Domain.Entites;
using Shortha.Domain.Enums;
using Shortha.Domain.Interfaces.Repositories;

namespace Shortha.Application.Services;

public class UpdatePackageDto
{
    public string? NewDetails { get; set; }
    public decimal Price { get; set; }
    public int Duration { get; set; }
}

public interface IPackagesService
{
    Task<PaginationResult<PackageInfoDto>> GetActivePackages();
    Task<Package> AddPackage(PackagesName name, string packageDetails, decimal price, int duration);
    Task RemovePackage(string packageId);
    Task DeactivatePackage(string packageId);
    Task<Package> GetPackageDetails(string packageId);
    Task UpdatePackage(UpdatePackageDto updatedPackage, string packageId);
}

public class PackagesService(IPackageRepository repo, IMapper mapper) : IPackagesService
{
    public async Task<Package> AddPackage(PackagesName name, string packageDetails, decimal price, int duration)
    {
        var package = new Package
        {
            Name = PackagesName.Free,
            Description = packageDetails,
            Price = price,
            DurationInDays = duration
        };
        await repo.AddAsync(package);
        await repo.SaveAsync();

        return package;
    }

    public async Task RemovePackage(string packageId)
    {
        var package = await repo.GetByIdAsync(packageId);
        if (package == null)
        {
            throw new NotFoundException($"Package with ID {packageId} does not exist.");
        }

        repo.Delete(package);
        await repo.SaveAsync();
    }

    public async Task DeactivatePackage(string packageId)
    {
        var package = await repo.GetByIdAsync(packageId);
        if (package == null)
        {
            throw new NotFoundException($"Package with ID {packageId} does not exist.");
        }

        package.IsActive = false;
        repo.Update(package);
        await repo.SaveAsync();
    }

    public async Task<Package> GetPackageDetails(string packageId)
    {
        var package = await repo.GetByIdAsync(packageId);
        if (package == null)
        {
            throw new NotFoundException($"Package with ID {packageId} does not exist.");
        }

        return package;
    }

    public async Task UpdatePackage(UpdatePackageDto updatedPackage, string packageId)
    {
        //1. Get the package by ID
        var package = await repo.GetByIdAsync(packageId);
        if (package == null)
        {
            throw new NotFoundException($"Package with ID {packageId} does not exist.");
        }

        //2. Update the package details
        if (!string.IsNullOrEmpty(updatedPackage.NewDetails))
        {
            package.Description = updatedPackage.NewDetails;
        }

        if (updatedPackage.Duration > 0)
        {
            package.DurationInDays = updatedPackage.Duration;
        }

        //3. Update the price
        if (updatedPackage.Price > 0)
        {
            package.Price = updatedPackage.Price;
        }

        //4. Save the changes
        repo.Update(package);
        await repo.SaveAsync();
    }

    public async Task<PaginationResult<PackageInfoDto>> GetActivePackages()
    {
        var packages = await repo.GetAsync(p => p.IsActive, 1, 10, p => p.CreatedAt, false);

        var mapped = mapper.Map<PaginationResult<PackageInfoDto>>(packages);

        return mapped;
    }
}