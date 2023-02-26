using Serilog;
using ThreadboxApi.Services;

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
				var hostEnvironment = services.GetRequiredService<IWebHostEnvironment>();
				var dbInitializationService = services.GetRequiredService<DbInitializationService>();

				await dbInitializationService.InitializeIfNotExists();
			}

			await host.RunAsync();
		}
	}
}