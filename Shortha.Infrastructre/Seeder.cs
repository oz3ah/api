using Microsoft.Extensions.DependencyInjection;
using Shortha.Domain.Entites;
using Shortha.Domain.Enums;

namespace Shortha.Infrastructre;

public static class Seeder
{
    public static async Task SeedPackagesAsync(IServiceProvider serviceProvider)
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