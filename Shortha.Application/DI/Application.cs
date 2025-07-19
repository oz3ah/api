using Microsoft.Extensions.DependencyInjection;

namespace Shortha.Application.DI;

public static class Application
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {

        services.AddServices();
        services.AddValidation();

        return services;
    }

}