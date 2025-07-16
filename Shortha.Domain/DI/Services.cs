using Microsoft.Extensions.DependencyInjection;
using Shortha.Domain.Services;

namespace Shortha.Domain.DI;

public static class Services
{
    
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        
        return services;
    }
    
}