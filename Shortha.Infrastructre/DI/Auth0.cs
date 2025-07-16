using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Shortha.Infrastructre.Auth0;
using System.Security.Claims;
using Shortha.Domain.Interfaces;
namespace Shortha.Infrastructre.DI
{
    internal static class Auth0
    {
        public static IServiceCollection AddAuth0(this IServiceCollection services)
        {
            var secretManager = services.BuildServiceProvider().GetRequiredService<ISecretService>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                                    .AddJwtBearer(options =>
                                    {
                                        options.Authority = secretManager.GetSecret("Authority");
                                        options.Audience = secretManager.GetSecret("Audience");
                                        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                                        {
                                            NameClaimType = ClaimTypes.NameIdentifier
                                        };
                                    });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(
                  "create:url",
                  policy => policy.Requirements.Add(
                    new HasScopeRequirement("create:url", secretManager.GetSecret("Authority"))
                  )
                );
            });

            services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();


            return services;
        }
    }
}
