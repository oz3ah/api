using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Shortha.Application.Interfaces;
using Shortha.Domain.Dto;
using Shortha.Domain.Interfaces;
using Shortha.Infrastructre.Authentication;
using System.Text;

namespace Shortha.Infrastructre.DI
{
    internal static class Auth
    {
        public static IServiceCollection AddAuthenticationService(this IServiceCollection services)
        {
            var sp = services.BuildServiceProvider();
            var secretManager = sp.GetRequiredService<ISecretService>();

            var jwtSecret = secretManager.GetSecret("JwtSecret");
            var jwtAudience = secretManager.GetSecret("JwtAudience");

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = "SmartScheme";
                    options.DefaultChallengeScheme = "SmartScheme";
                })
                .AddPolicyScheme("SmartScheme", "JWT or API Key", options =>
                {
                    options.ForwardDefaultSelector = context =>
                    {
                        // Check API key
                        if (context.Request.Headers.ContainsKey("x-api-key"))
                            return "ApiKey";

                        // Otherwise fallback to JWT
                        return JwtBearerDefaults.AuthenticationScheme;
                    };
                })
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,

                        ValidateAudience = true,
                        ValidAudience = jwtAudience,

                        ValidateLifetime = true,

                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),

                        NameClaimType = ClaimTypes.NameIdentifier
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnChallenge = context =>
                        {
                            context.HandleResponse();
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            context.Response.ContentType = "application/json";

                            var error = ErrorResponse.From("Unauthorized access",
                                traceId: context.HttpContext.TraceIdentifier);
                            return context.Response.WriteAsJsonAsync(error);
                        }
                    };
                })
                .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>("ApiKey", null);


            return services;
        }
    }
}