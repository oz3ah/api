using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Sinks.Grafana.Loki;
using Shortha.Domain.Interfaces;
using Shortha.Infrastructre.Serilog;

namespace Shortha.Infrastructre.DI;

public static class SerilogConfiguration
{
    public static void AddSerilogLogging(this IHostBuilder builder, IServiceProvider serviceProvider)
    {


        builder.UseSerilog((context, services, config) =>
        {
            var secretService = serviceProvider.GetService<ISecretService>();

            // Basic enrichment
            config.Enrich.FromLogContext();
            config.Enrich.WithExceptionDetails();
            // Custom enricher with HTTP context information
            var httpContextAccessor = services.GetService<IHttpContextAccessor>();
            if (httpContextAccessor != null)
            {
                config.Enrich.With(new SerilogEnricher(
                                                       Config.AppId,
                                                       Config.AppName,
                                                       Config.Env,
                                                       httpContextAccessor));
            }
          

            // Enhanced console output with more detailed template
            config
                .WriteTo.Console(
                                 outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}] " +
                                                 "[{Level:u3}] " +
                                                 "[{CorrelationId}] " +
                                                 "[{ApplicationId}/{Environment}] " +
                                                 "[{RequestMethod} {RequestPath}] " +
                                                 "[User: {UserId}] " +
                                                 "[IP: {ClientIpAddress}] " +
                                                 "{Message:lj} " +
                                                 "{NewLine}{Exception}");

            // Configure Loki sink with authorization
            ConfigureLokiSink(config, secretService);

            // Set minimum log levels
            config.MinimumLevel.Information();
            config.MinimumLevel.Override("Microsoft", LogEventLevel.Warning);
            config.MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information);
            config.MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning);

            // Read additional configuration from appsettings
            config.ReadFrom.Configuration(context.Configuration);
        });
    }

    private static void ConfigureLokiSink(LoggerConfiguration config, ISecretService? secretService)
    {
        try
        {
            if (secretService == null)
            {
                Console.WriteLine("❌ Warning: ISecretService is not available, Loki sink will not be configured");
                return;
            }

            var lokiUrl = "https://logs-prod-039.grafana.net";
            var labels = new List<LokiLabel>
                         {
                             new() { Key = "appId", Value = Config.AppId },
                             new() { Key = "appName", Value = Config.AppName },
                             new() { Key = "env", Value = Config.Env },
                             new() { Key = "service", Value = "shortha-api" }
                         };


            var lokiUsername = secretService.GetSecret("LokiUsername");
            var lokiPassword = secretService.GetSecret("LokiPassword");


            // Configure with basic authentication
            config.WriteTo.GrafanaLoki(
                                       uri: lokiUrl,
                                       labels: labels,
                                       credentials: new LokiCredentials
                                       {
                                           Login = lokiUsername,
                                           Password = lokiPassword
                                       },
                                       period: TimeSpan.FromSeconds(2)
                                      );
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to configure Loki sink: {ex.Message}");
        }
    }
}