using Microsoft.Extensions.DependencyInjection;
using Shortha.Application.Interfaces.Services;
using Shortha.Application.Services;

namespace Shortha.Application.DI;

internal static class Services
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUrlService, UrlService>();
        services.AddScoped<IVisitService, VisitService>();
        services.AddScoped<IPackagesService, PackagesService>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<ISubscriptionService, SubscriptionService>();
        services.AddScoped<IKahsierService, KahsierService>();
        services.AddScoped<IAnalyticsService, AnalyticsService>();
        services.AddScoped<IApiKeyService, ApiKeyService>();


        return services;
    }
}