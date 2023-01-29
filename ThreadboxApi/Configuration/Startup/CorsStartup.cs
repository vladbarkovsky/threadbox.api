namespace ThreadboxApi.Configuration.Startup
{
	public class CorsStartup
	{
		public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
		{
			services.AddCors(options =>
			{
				options.AddDefaultPolicy(builder =>
				{
					builder
						.WithOrigins(configuration[AppSettings.CorsOrigins]!.Split(", "))
						// NOTE: CORS allows simple methods (GET, HEAD, POST) regardless of
						// Access-Control-Allow-Methods header content
						// Source: https://stackoverflow.com/a/44385327
						.WithMethods(configuration[AppSettings.CorsMethods]!.Split(", "))
						.WithHeaders(configuration[AppSettings.CorsHeaders]!.Split(", "))
						.Build();
				});
			});
		}

		public static void Configure(IApplicationBuilder app)
		{
			app.UseCors();
		}
	}
}