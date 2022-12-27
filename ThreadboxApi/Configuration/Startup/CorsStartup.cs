﻿using ThreadboxApi.Configuration;

namespace ThreadboxApi.Configuration.Startup
{
	public class CorsStartup
	{
		public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
		{
			services.AddCors(options =>
			{
				options.AddPolicy(configuration[AppSettings.CorsPolicy]!, builder =>
				{
					builder
						.WithOrigins(configuration[AppSettings.CorsOrigins]!.Split(", "))
						// TODO: CORS don't restrict methods that are no t allowed
						.WithMethods(configuration[AppSettings.CorsMethods]!.Split(", "))
						.WithHeaders(configuration[AppSettings.CorsHeaders]!.Split(", "))
						.Build();
				});
			});
		}

		public static void Configure(WebApplication app)
		{
			app.UseCors(app.Configuration[AppSettings.CorsPolicy]!);
		}
	}
}