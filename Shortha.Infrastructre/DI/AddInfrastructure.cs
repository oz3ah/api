using Microsoft.Extensions.DependencyInjection;
using Shortha.Infrastructre.Interceptors;

namespace Shortha.Infrastructre.DI;

public static class Infrastructure
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSecretManager();
        services.AddIpTracker();
        services.AddAuthenticationService();
        services.AddDatabase();
        services.AddRepositories();
        services.AddSingleton<SoftDeleteInterceptor>();
        services.AddSingleton<AuditableInterceptor>();
        services.RegisterHangfire();

        return services;
    }
}