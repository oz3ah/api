using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shortha.Domain.Interfaces;
using Shortha.Infrastructre.Interceptors;

namespace Shortha.Infrastructre.DI
{
    internal static class DB
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services)
        {
            services.AddDbContext<AppDb>((serviceProvider, options) =>
            {
                var secretService = serviceProvider.GetRequiredService<ISecretService>();
                var softDeleteInterceptor = serviceProvider.GetRequiredService<SoftDeleteInterceptor>();
                var updateInterceptor = serviceProvider.GetRequiredService<UpdateTimestampInterceptor>();
                options
                    .UseNpgsql(secretService.GetSecret("Db"))
                    .EnableDetailedErrors()
                    .EnableSensitiveDataLogging();
                // The interceptor is injected via AppDb constructor
                options.AddInterceptors(softDeleteInterceptor);
                options.AddInterceptors(updateInterceptor);
            });

            return services;
        }
    }
}
