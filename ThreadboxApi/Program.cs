using Serilog;
using Serilog.Formatting.Display;
using Serilog.Events;
using ThreadboxApi.ORM.Services;

namespace ThreadboxApi
{
    public class Program
    {
        private static async Task Main(string[] args)
        {
            using IHost host = Host
                .CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(void (HostBuilderContext hostBuilderContext, IConfigurationBuilder configurationBuilder) =>
                {
                    configurationBuilder.AddJsonFile(
                        "appsettings.json",
                        optional: false,
                        reloadOnChange: true);
                    configurationBuilder.AddJsonFile(
                        $"appsettings.{hostBuilderContext.HostingEnvironment.EnvironmentName}.json",
                        optional: true,
                        reloadOnChange: true);

                    configurationBuilder.AddEnvironmentVariables();

                    if (args != null)
                    {
                        configurationBuilder.AddCommandLine(args);
                    }
                })
                .ConfigureWebHostDefaults(void (IWebHostBuilder builder) => builder.UseStartup<Startup>())
                .UseSerilog(void (HostBuilderContext hostBuilderContext, LoggerConfiguration configuration) =>
                {
                    configuration.WriteTo.File(
                        restrictedToMinimumLevel: LogEventLevel.Warning,
                        path: hostBuilderContext.Configuration["LogPath"],
                        formatter: new MessageTemplateTextFormatter("[{Timestamp:HH:mm:ss.fff} {Level:u3}] {TraceId} {SourceContext}: {Message}{NewLine}{Exception}"),
                        rollingInterval: RollingInterval.Day,
                        retainedFileCountLimit: 7);

                    configuration.WriteTo.Console();
                    configuration.Enrich.FromLogContext();
                })
                .Build();

            using (IServiceScope scope = host.Services.CreateScope())
            {
                RoleService roleService = scope.ServiceProvider.GetRequiredService<RoleService>();
                await roleService.SynchronizeRolesAsync();
            }

            await host.RunAsync();
        }
    }
}
