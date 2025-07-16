using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Shortha.Infrastructre.Auth0;
using System.Security.Claims;

namespace Shortha.Infrastructre.DI
{
    public static class Auth0
    {
        public static IServiceCollection AddAuth0(this IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                                    .AddJwtBearer(options =>
                                    {
                                        options.Authority = $"https://dev-r13gyp2kxbjasb87.us.auth0.com/";
                                        options.Audience = "https://sortha.api";
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
                    new HasScopeRequirement("create:url", "https://dev-r13gyp2kxbjasb87.us.auth0.com/")
                  )
                );
            });

            services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();


            return services;
        }
    }
}
