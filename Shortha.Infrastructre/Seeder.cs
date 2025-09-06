using Microsoft.Extensions.DependencyInjection;
using Shortha.Domain.Entites;
using Shortha.Domain.Enums;

namespace Shortha.Infrastructre;

public static class Seeder
{
    public static void Seed(IServiceProvider serviceProvider)
    {
        SeedPackagesAsync(serviceProvider).GetAwaiter().GetResult();
        SeedRolesAsync(serviceProvider).GetAwaiter().GetResult();
    }

    private static async Task SeedRolesAsync(IServiceProvider serviceProvider)
    {
        var dbProvider = serviceProvider.GetRequiredService<AppDb>();
        // Check if the roles already exist
        if (dbProvider.Roles.Any())
        {
            return; // Roles already exist, no need to seed
        }

        dbProvider.Roles.Add(new Role
        {
            Name = "FREE",
            CreatedAt = DateTime.UtcNow
        });
        dbProvider.Roles.Add(new Role
        {
            Name = "PRO",
            CreatedAt = DateTime.UtcNow
        });
        await dbProvider.SaveChangesAsync();
    }


    private static async Task SeedPackagesAsync(IServiceProvider serviceProvider)
    {
        var dbProvider = serviceProvider.GetRequiredService<AppDb>();
        // Check if the packages already exist
        if (dbProvider.Packages.Any())
        {
            return; // Packages already exist, no need to seed
        }

        dbProvider.Packages.Add(new Package
        {
            Name = PackagesName.Pro,
            Price = 114.0m,
            MaxUrls = -1,
            CreatedAt = DateTime.UtcNow,
            Description =
                "Built for professionals, marketers, and businesses who need more control, customization, and integrations.",
            DurationInDays = 36500
        });

        dbProvider.Packages.Add(new Package
        {
            Name = PackagesName.Free,
            Price = 0.0m,
            MaxUrls = -1,
            CreatedAt = DateTime.UtcNow,
            Description = "Perfect for individuals and casual users who want fast, simple link shortening.",
            DurationInDays = 36500
        });

        await dbProvider.SaveChangesAsync();
    }
}