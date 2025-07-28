using Microsoft.Extensions.DependencyInjection;
using Shortha.Infrastructre.Interceptors;

namespace Shortha.Infrastructre.DI;

public static class Infrastructure
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddTracing();
        services.AddSecretManager();
        services.AddIpTracker();
        services.AddAuth0();
        services.AddDatabase();
        services.AddRepositories();
        services.AddSingleton<SoftDeleteInterceptor>();
        services.AddSingleton<AuditableInterceptor>();
        services.AddApiClients();
        services.RegisterHangfire();

        return services;
    }
}