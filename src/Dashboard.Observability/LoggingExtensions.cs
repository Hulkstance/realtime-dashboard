using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace Dashboard.Observability;

public static class LoggingExtensions
{
    public static IHostBuilder AddDashboardLogging(
        this IHostBuilder builder,
        LogEventLevel minLevelLocal = LogEventLevel.Information)
    {
        const string outputTemplate = "[{Timestamp:HH:mm:ss.fff} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}";

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Is(minLevelLocal)
            .MinimumLevel.Override("System", LogEventLevel.Error)
            .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .Enrich.WithProcessName()
            .Enrich.WithMemoryUsage()
            .WriteTo.Console(outputTemplate: outputTemplate)
            .CreateLogger();

        return builder.UseSerilog();
    }
}
