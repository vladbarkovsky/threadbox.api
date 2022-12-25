namespace ThreadboxApi.Configuration.Startup
{
	public class SwaggerStartup
	{
		public static void ConfigureServices(IServiceCollection services)
		{
			services.AddSwaggerDocument(settings =>
			{
				settings.Title = "Threadbox API";
			});
		}

		public static void Configure(WebApplication app)
		{
			if (app.Environment.IsDevelopment())
			{
				app.UseOpenApi();
				app.UseSwaggerUi3();
			}
		}
	}
}