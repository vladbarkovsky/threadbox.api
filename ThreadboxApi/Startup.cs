using System.Reflection;
using ThreadboxApi.Web.Startup;

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

            services
                .AddMvc()
                .AddRazorPagesOptions(options =>
                {
                    options.RootDirectory = "/Web/IdentityServer/Views";
                });

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
            app.UseStaticFiles();
            SwaggerStartup.Configure(app, _webHostEnvironment);

            /// IMPORTANT: CORS must be configured before
            /// <see cref="ControllerEndpointRouteBuilderExtensions.MapControllers"/>,
            /// <see cref="AuthorizationAppBuilderExtensions.UseAuthorization"/>,
            /// <see cref="HttpsPolicyBuilderExtensions.UseHttpsRedirection"/>,

            app.UseCors();
            app.UseExceptionHandler();
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