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
using Microsoft.AspNetCore.Mvc; // added for ApiBehaviorOptions

namespace Shortha
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers(options =>
            {
                options.Filters.Add<ValidationFilter>();
            });
            builder.Services.Configure<ApiBehaviorOptions>(o =>
            {
                o.SuppressModelStateInvalidFilter = true;
            });
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



            // Add enhanced logging AFTER all services are registered
            builder.Host.AddSerilogLogging();

            builder.Services.AddDocs();

            var app = builder.Build();

            app.UseMiddleware<PerformanceMonitoringMiddleware>();
            
            app.UseMiddleware<RequestResponseLoggingMiddleware>();
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
                    var error = ErrorResponse.From("Access denied", traceId: context.TraceIdentifier, 
                        path: context.Request.Path.Value, statusCode: StatusCodes.Status403Forbidden);
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsJsonAsync(error);
                }
            });

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                Seeder.SeedPackagesAsync(services).Wait();
            }

            app.UseHangfireDashboard("/dashboard");
            app.UseHttpsRedirection();
            app.UseSerilogRequestLogging(options =>
            {
                options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms | CorrelationId: {CorrelationId}";
                options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
                {
                    diagnosticContext.Set("CorrelationId", httpContext.TraceIdentifier);
                    diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
                    diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
                    diagnosticContext.Set("UserAgent", httpContext.Request.Headers["User-Agent"].ToString());
                    
                    if (httpContext.User?.Identity?.IsAuthenticated == true)
                    {
                        diagnosticContext.Set("UserId", httpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
                    }
                };
            });
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            Log.Information("Shortha API starting up | Environment: {Environment} | AppId: {AppId}", 
                builder.Environment.EnvironmentName, Shortha.Infrastructre.Config.AppId);

     

            app.Run();
        }
    }
}