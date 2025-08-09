using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Grafana.Loki;
using Shortha.Infrastructre.Serilog;

namespace Shortha.Infrastructre.DI;

public static class SerilogConfiguration
{
    public static void AddSerilogLogging(this IHostBuilder builder)
    {
        builder.UseSerilog((context, config) =>
        {
            config.Enrich.FromLogContext();
            config.Enrich.WithExceptionDetails();
            config.Enrich.With(new SerilogEnricher(
                                                   Config.AppId,
                                                   Config.AppName,
                                                   Config.Env
                                                  ));

            config
                .WriteTo.GrafanaLoki
                    (
                     "https://loki.gitnasr.com",
      
                     new List<LokiLabel> { new() { Key = "appId", Value = Config.AppId }, new() { Key = "appName", Value =
                                             Config.AppName
                                         }, new() { Key = "env", Value = Config.Env } },
                     period: TimeSpan.Zero
                    )
                .WriteTo.Console
                    (
                     outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} level=[{Level:u3}] appId={ApplicationId} appName={ApplicationName} env={Environment} {Message:lj} {NewLine}{Exception}"
                    );
            config.ReadFrom.Configuration(context.Configuration);
        });


    }
}