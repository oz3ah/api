using IPinfo;
using Microsoft.Extensions.DependencyInjection;
using Shortha.Domain.Interfaces;

namespace Shortha.Infrastructre.DI;

public static class AddIpClient
{
    internal static IServiceCollection AddIpTracker(this IServiceCollection services)
    {
        services.AddSingleton(provider =>
        {
            // Get Secret  Service
            var secretService = provider.GetRequiredService<ISecretService>();

            var token = secretService.GetSecret("IpInfoKey");
            return new IPinfoClient.Builder()
                   .AccessToken(token)
                   .Build();
        });
        return services;
    }
}