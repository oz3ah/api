using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
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
                var auditableInterceptor = serviceProvider.GetRequiredService<AuditableInterceptor>();
                options
                    .UseNpgsql(secretService.GetSecret("Db"))
                    .EnableDetailedErrors()
                    .EnableSensitiveDataLogging();

                options.AddInterceptors(softDeleteInterceptor);
                options.AddInterceptors(auditableInterceptor);
            });
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            NpgsqlConnection.GlobalTypeMapper.EnableDynamicJson();

            return services;
        }
    }
}