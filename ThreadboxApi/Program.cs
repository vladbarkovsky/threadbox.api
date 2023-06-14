using Serilog;
using ThreadboxApi.Infrastructure.Persistence;
using ThreadboxApi.Infrastructure.Persistence.Seed;
using ThreadboxApi.Web;

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
                    loggerConfiguration.ReadFrom.Configuration(hostBuilderContext.Configuration);
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