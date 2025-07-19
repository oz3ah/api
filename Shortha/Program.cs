using Scalar.AspNetCore;
using Serilog;
using Shortha.Application.DI;
using Shortha.Application.Dto.AutoMapper;
using Shortha.Domain.Dto;
using Shortha.Extenstions;
using Shortha.Filters;
using Shortha.Infrastructre.DI;
using Shortha.Middleware;

namespace Shortha
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddAutoMapper(typeof(UrlConfiguration));

            builder.Services.AddSwaggerGen();
            builder.Services.AddInfrastructure();
            builder.Services.AddApplication();
            builder.Services.AddScoped<TrackerFilter>();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder => builder.AllowAnyOrigin()
                                      .AllowAnyMethod()
                                      .AllowAnyHeader());
            });

            builder.Host.AddSerilogLogging();

            builder.Services.AddDocs();

            var app = builder.Build();
            app.UseOpenTelemetryPrometheusScrapingEndpoint();

            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseCors();
            app.UseSwagger(opt => { opt.RouteTemplate = "openapi/{documentName}.json"; });
            app.MapScalarApiReference(opt =>
            {
                opt.Title = "Shortha API";
                opt.Theme = ScalarTheme.DeepSpace;
                opt.DefaultHttpClient = new(ScalarTarget.Http, ScalarClient.Http11);
            });
            app.Use(async (context, next) =>
            {
                await next();

                if (context.Response.StatusCode == StatusCodes.Status403Forbidden && !context.Response.HasStarted)
                {
                    var error = ErrorResponse.From("Access denied", traceId: context.TraceIdentifier);
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsJsonAsync(error);
                }
            });

            // Configure the HTTP request pipeline.
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();
            app.UseSerilogRequestLogging();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
