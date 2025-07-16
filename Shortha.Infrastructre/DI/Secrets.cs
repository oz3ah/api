using dotenv.net;
using Infisical.Sdk;
using Microsoft.Extensions.DependencyInjection;
using Shortha.Domain.Interfaces;
using static Shortha.Infrastructre.Secrets.Manager;

namespace Shortha.Infrastructre.DI
{
    internal static class Secrets
    {
        public static IServiceCollection AddSecretManager(this IServiceCollection services)
        {
            DotEnv.Load();

            services.AddSingleton<InfisicalClient>(_ =>
            {
                var clientId = Environment.GetEnvironmentVariable("ClientId");
                var clientSecret = Environment.GetEnvironmentVariable("ClientSecret");

                if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(clientSecret))
                    throw new
                        InvalidOperationException("Infisical credentials not configured in environment variables.");

                var settings = new ClientSettings
                {
                    Auth = new AuthenticationOptions
                    {
                        UniversalAuth = new UniversalAuthMethod
                        {
                            ClientId = clientId,
                            ClientSecret = clientSecret
                        }
                    }
                };

                return new InfisicalClient(settings);
            });

            services.AddSingleton<ISecretService, SecretService>();

            return services;
        }

    }
}
