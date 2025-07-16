using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shortha.Domain.Interfaces;

namespace Shortha.Infrastructre.DI
{
    internal static class DB
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services)
        {
            services.AddDbContext<AppDb>((serviceProvider, options) =>
            {
                var secretService = serviceProvider.GetRequiredService<ISecretService>();
                options
                    .UseNpgsql(secretService.GetSecret("Db"))
                    .EnableDetailedErrors()
                    .EnableSensitiveDataLogging();
            });

            return services;
        }
    }
}
