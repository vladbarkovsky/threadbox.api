using Serilog;
using Serilog.Formatting.Display;
using Serilog.Events;
using ThreadboxApi.Infrastructure.Persistence.Seeding;

namespace ThreadboxApi
{
    public class Program
    {
        private static async Task Main(string[] args)
        {
            var host = Host
                .CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .UseSerilog((hostBuilderContext, loggerConfiguration) =>
                {
                    loggerConfiguration
                        .WriteTo.File(
                            restrictedToMinimumLevel: LogEventLevel.Warning,
                            path: "Logs/.log",
                            formatter: new MessageTemplateTextFormatter("[{Timestamp:HH:mm:ss.fff} {Level:u3}] {SourceContext}: {Message}{NewLine}{Exception}"),
                            rollingInterval: RollingInterval.Day,
                            retainedFileCountLimit: 7)
                        .WriteTo.Console()
                        .Enrich.FromLogContext();
                })
                .Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var dbInitializationService = services.GetRequiredService<DbInitializationService>();
                await dbInitializationService.EnsureInitializedAsync();

                await host.RunAsync();
            }
        }
    }
}