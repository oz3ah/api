using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.Extensions.DependencyInjection;
using Shortha.Application.Interfaces;
using Shortha.Domain.Interfaces;
using Shortha.Infrastructre.Background_Jobs;

namespace Shortha.Infrastructre.DI
{
    internal static class Hangfire
    {
        public static IServiceCollection RegisterHangfire(this IServiceCollection services)
        {
            var secretService = services.BuildServiceProvider().GetRequiredService<ISecretService>();
            services.AddHangfire(config =>
            {
                config.UseSimpleAssemblyNameTypeSerializer();
                config.UseRecommendedSerializerSettings();
                config.UsePostgreSqlStorage(options => options.UseNpgsqlConnection(secretService.GetSecret("Db")));

            });
            services.AddHangfireServer();
            services.AddScoped<IBackgroundJobService, HangfireJobService>();

            return services;
        }
    }
}
