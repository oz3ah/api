using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Shortha.Application.Interfaces;
using Shortha.Authentication;
using Shortha.Domain.Dto;
using Shortha.Domain.Interfaces;
using Shortha.Infrastructre.Auth0;

namespace Shortha.Infrastructre.DI
{
    internal static class Auth0
    {
        public static IServiceCollection AddAuth0(this IServiceCollection services)
        {
            var secretManager = services.BuildServiceProvider().GetRequiredService<ISecretService>();
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = "SmartScheme";
                    options.DefaultChallengeScheme = "SmartScheme";
                })
                .AddPolicyScheme("SmartScheme", "JWT or API Key", options =>
                {
                    options.ForwardDefaultSelector = context =>
                    {
                        if (context.Request.Headers.ContainsKey("X-API-Key".ToLower()))
                            return "ApiKey";

                        return JwtBearerDefaults.AuthenticationScheme;
                    };
                })
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
                            context.HandleResponse();
                            context.Response.StatusCode =
                                StatusCodes.Status401Unauthorized;
                            context.Response.ContentType = "application/json";

                            var error = ErrorResponse.From("Unauthorized access",
                                traceId: context.HttpContext.TraceIdentifier);
                            return context.Response.WriteAsJsonAsync(error);
                        }
                    };
                }).AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>("ApiKey", null);
            ;

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