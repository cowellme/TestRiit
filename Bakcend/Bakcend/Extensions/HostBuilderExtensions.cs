using Serilog;

namespace Backend.Extensions;

public static class HostBuilderExtensions
{
    public static void UseAppSerilog(this IHostBuilder host)
    {
        host.UseSerilog((context, config) =>
        {
            config
                .MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.File("logs/app-.log",
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 14);
        });
    }
}