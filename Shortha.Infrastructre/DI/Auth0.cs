using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Shortha.Infrastructre.Auth0;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Shortha.Application.Interfaces;
using Shortha.Domain.Dto;
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
                                        options.Events = new JwtBearerEvents
                                        {
                                            OnChallenge = context =>
                                            {
                                                context.HandleResponse(); // Prevent default behavior
                                                context.Response.StatusCode =
                                                    StatusCodes.Status401Unauthorized;
                                                context.Response.ContentType = "application/json";

                                                var error = ErrorResponse.From("Unauthorized access",
                                                 traceId: context.HttpContext.TraceIdentifier);
                                                return context.Response.WriteAsJsonAsync(error);
                                            }
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

            services.AddScoped<IAuth0ManagementService, Auth0ManagementService>();

            return services;
        }
    }
}
