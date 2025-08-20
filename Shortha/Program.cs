using Hangfire;
using Scalar.AspNetCore;
using Serilog;
using Shortha.Application.AutoMapper;
using Shortha.Application.DI;
using Shortha.Domain.Dto;
using Shortha.Domain.Interfaces;
using Shortha.Extenstions;
using Shortha.Filters;
using Shortha.Infrastructre;
using Shortha.Infrastructre.DI;
using Shortha.Middleware;
using Shortha.Providers;

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
            builder.Services.AddAutoMapper(typeof(Default).Assembly);

            builder.Services.AddSwaggerGen();
            builder.Services.AddInfrastructure();
            builder.Services.AddApplication();
            builder.Services.AddScoped<TrackerFilter>();
            builder.Services.AddCors(c =>
            {
                c.AddDefaultPolicy(d => d.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            });
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<ICurrentSessionProvider, CurrentSessionProvider>();

            builder.Host.AddSerilogLogging();

            builder.Services.AddDocs();

            var app = builder.Build();

            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseCors();
            app.UseSwagger(opt => { opt.RouteTemplate = "openapi/{documentName}.json"; });
            app.MapScalarApiReference(opt =>
            {
                opt.Title = "Shortha API";
                opt.Theme = ScalarTheme.DeepSpace;
                opt.DefaultHttpClient = new(ScalarTarget.Http, ScalarClient.Curl);
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

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                Seeder.SeedPackagesAsync(services).Wait();
            }

            // Configure the HTTP request pipeline.
            app.UseHangfireDashboard("/dashboard");
            app.UseHttpsRedirection();
            app.UseSerilogRequestLogging();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}