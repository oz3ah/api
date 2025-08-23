using Microsoft.Extensions.DependencyInjection;
using Shortha.Domain.Interfaces;
using Shortha.Domain.Interfaces.Repositories;
using Shortha.Infrastructre.Repositories;
using Shortha.Infrastructre.Units;

namespace Shortha.Infrastructre.DI;

public static class Repositories
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUrlRepository, UrlRepository>();
        services.AddScoped<IVisitRepository, VisitRepository>();
        services.AddScoped<IPackageRepository, PackageRepository>();

        services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddScoped<IApiRepository, ApiRepository>();
        services.AddScoped<IAppConnectionRepository, AppConnectionRepository>();
        services.AddScoped<IUnitOfWork, EfUnitOfWork>();

        return services;
    }
}