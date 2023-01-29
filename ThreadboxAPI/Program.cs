using Serilog;
using ThreadboxApi.Configuration;
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
				var dbSeedingService = services.GetRequiredService<DbSeedingService>();

				try
				{
					if (hostEnvironment.IsDevelopment())
					{
						await dbSeedingService.SeedDbAsync();
					}
					else if (hostEnvironment.IsProduction())
					{ }
				}
				catch (Exception ex)
				{
					var logger = services.GetRequiredService<ILogger<Program>>();
					logger.LogError(ex, "An error occurred while seeding the database.");
					throw;
				}
			}

			await host.RunAsync();
		}
	}
}