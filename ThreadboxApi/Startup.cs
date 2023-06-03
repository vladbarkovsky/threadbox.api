using System.Reflection;
using ThreadboxApi.Configuration;
using ThreadboxApi.Configuration.Startup;
using ThreadboxApi.Configuraton.Startup;

namespace ThreadboxApi
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            DbStartup.ConfigureServices(services, _configuration);
            DependencyInjectionStartup.ConfigureServices(services);
            services.AddControllers();
            SwaggerStartup.ConfigureServices(services);
            CorsStartup.ConfigureServices(services, _configuration);
            ExceptionHandlingStartup.ConfigureServices(services);
            IdentityStartup.ConfigureServices(services, _configuration);
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            MediatRStartup.ConfigureServices(services);
            FluentValidationStartup.ConfigureServices(services);
        }

        public void Configure(IApplicationBuilder app)
        {
            SwaggerStartup.Configure(app, _webHostEnvironment);

            /// IMPORTANT: CORS must be configured before
            /// <see cref="ControllerEndpointRouteBuilderExtensions.MapControllers"/>,
            /// <see cref="AuthorizationAppBuilderExtensions.UseAuthorization"/>,
            /// <see cref="HttpsPolicyBuilderExtensions.UseHttpsRedirection"/>,

            CorsStartup.Configure(app);
            ExceptionHandlingStartup.Configure(app);
            app.UseMiddleware<JwtAccessTokenRefreshMiddleware>();
            app.UseRouting();
            IdentityStartup.Configure(app);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseHttpsRedirection();
        }
    }
}