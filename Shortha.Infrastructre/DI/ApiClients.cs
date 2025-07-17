using Microsoft.Extensions.DependencyInjection;
using Refit;
using Shortha.Domain.Interfaces;
using Shortha.Infrastructre.Auth0;

namespace Shortha.Infrastructre.DI;

internal static class ApiClients
{
    
    public static IServiceCollection AddApiClients(this IServiceCollection services)
    {
        services.AddRefitClient<IAuth0UserInfoApi>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://dev-r13gyp2kxbjasb87.us.auth0.com/"));
services.AddScoped<IAuth0UserInfoService, Auth0UserInfoService>();
        return services;
    }
    
}