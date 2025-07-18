using Microsoft.Extensions.DependencyInjection;
using Shortha.Infrastructre.Interceptors;

namespace Shortha.Infrastructre.DI;

public static class Infrastructure
{

    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddTracing();
        services.AddSecretManager();
        services.AddAuth0();
        services.AddDatabase();
        services.AddRepositories();
        services.AddSingleton<SoftDeleteInterceptor>();
        services.AddSingleton<UpdateTimestampInterceptor>();
        services.AddApiClients();
        
        return services;
    }
    
}