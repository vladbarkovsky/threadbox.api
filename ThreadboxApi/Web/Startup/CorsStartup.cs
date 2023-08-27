using ThreadboxApi.Application.Common;

namespace ThreadboxApi.Web.Startup
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
                        .WithOrigins(configuration[AppSettings.ClientBaseUrl])
                        // NOTE: CORS allows simple methods (GET, HEAD, POST) regardless of
                        // Access-Control-Allow-Methods header content
                        // Source: https://stackoverflow.com/a/44385327
                        .WithMethods("PUT", "DELETE")
                        .WithHeaders("Content-Type", "Authorization")
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