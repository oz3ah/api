using Microsoft.Extensions.DependencyInjection;
using Shortha.Domain.Interfaces.Repositories;
using Shortha.Infrastructre.Repositories;

namespace Shortha.Infrastructre.DI;

public static class Repositories
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUrlRepository, UrlRepository>();

        return services;
    }

}