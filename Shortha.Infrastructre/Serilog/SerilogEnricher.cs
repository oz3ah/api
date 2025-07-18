using Serilog.Core;
using Serilog.Events;

namespace Shortha.Infrastructre.Serilog;



public class SerilogEnricher(string applicationId, string applicationName, string environmentName)
    : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("ApplicationId", applicationId));
        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("ApplicationName", applicationName));
        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("Environment", environmentName));
    }
}