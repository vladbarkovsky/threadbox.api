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

		public static void Configure(IApplicationBuilder app, IWebHostEnvironment webHostEnvironment)
		{
			if (webHostEnvironment.IsDevelopment())
			{
				app.UseOpenApi();
				app.UseSwaggerUi3();
			}
		}
	}
}