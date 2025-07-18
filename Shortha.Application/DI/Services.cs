

using Microsoft.Extensions.DependencyInjection;
using Shortha.Application.Services;

namespace Shortha.Application.DI;

internal static class Services
{

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUrlService, UrlService>();

        return services;
    }

}