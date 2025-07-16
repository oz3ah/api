using Microsoft.Extensions.DependencyInjection;

namespace Shortha.Infrastructre.DI;

public static class Infrastructure
{

    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSecretManager();
        services.AddAuth0();
        services.AddDatabase();
        
        return services;
    }
    
}