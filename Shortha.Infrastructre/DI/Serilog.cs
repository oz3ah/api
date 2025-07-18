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
    Config.appId,
    Config.appName,
    Config.env
  ));

  config
    .WriteTo.GrafanaLoki
    (
      "https://loki.gitnasr.com",
      
      new List<LokiLabel> { new() { Key = "appId", Value = Config.appId }, new() { Key = "appName", Value =
                                                                             Config.appName
                                                                           }, new() { Key = "env", Value = Config.env } },
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