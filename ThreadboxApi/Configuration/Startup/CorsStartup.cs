namespace ThreadboxApi.Configuration.Startup
{
    public class CorsStartup
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(configuration[AppSettings.CorsPolicy], builder =>
                {
                    builder
                        .WithOrigins(configuration[AppSettings.CorsOrigins].Split(' '))
                        .WithMethods(configuration[AppSettings.CorsMethods].Split(' '))
                        .Build();
                });
            });
        }

        public static void Configure(WebApplication app)
        {
            app.UseCors(app.Configuration[AppSettings.CorsPolicy]);
        }
    }
}