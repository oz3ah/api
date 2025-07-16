using Microsoft.OpenApi.Models;

namespace Shortha.Extenstions;

public static class Docs
{
    public static IServiceCollection AddDocs(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                BearerFormat = "JWT",
                Description =
                                                            "JWT Authorization header using the Bearer scheme.",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer"
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
                                           {
                                               {
                                                   new OpenApiSecurityScheme
                                                   {
                                                       Reference = new OpenApiReference
                                                                   {
                                                                       Id = "Bearer",
                                                                       Type = ReferenceType.SecurityScheme
                                                                   }
                                                   },
                                                   Array.Empty<string>()
                                               }
                                           });
        });

        return services;
    }

}