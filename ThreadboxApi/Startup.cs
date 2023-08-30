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
            CorsStartup.ConfigureServices(services, _configuration);
            SwaggerStartup.ConfigureServices(services);
            ExceptionHandlingStartup.ConfigureServices(services);
            IdentityStartup.ConfigureServices(services, _configuration);
            IdentityServerStartup.ConfigureServices(services, _configuration, _webHostEnvironment);
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            MediatRStartup.ConfigureServices(services);
            FluentValidationStartup.ConfigureServices(services);

            services.AddMvc();

            // Disable redirecting for unauthorized HTTP requests
            // Source: https://stackoverflow.com/a/45271981/19232404
            services.ConfigureApplicationCookie(options =>
            {
                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Task.CompletedTask;
                };

                // May be required soon

                //options.Events.OnRedirectToAccessDenied = context =>
                //{
                //    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                //    return Task.CompletedTask;
                //};
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            /// IMPORTANT: CORS must be configured before
            /// <see cref="ControllerEndpointRouteBuilderExtensions.MapControllers"/>,
            /// <see cref="AuthorizationAppBuilderExtensions.UseAuthorization"/>,
            /// <see cref="HttpsPolicyBuilderExtensions.UseHttpsRedirection"/>,
            CorsStartup.Configure(app);

            SwaggerStartup.Configure(app, _webHostEnvironment);
            ExceptionHandlingStartup.Configure(app);

            // Enable HTTP routing
            app.UseRouting();

            IdentityStartup.Configure(app);
            IdentityServerStartup.Configure(app);

            /// IMPORTANT: CSP must be configured before
            /// <see cref="EndpointRoutingApplicationBuilderExtensions.UseEndpoints(IApplicationBuilder, Action{IEndpointRouteBuilder})"/>
            CspStartup.Configure(app, _configuration, _webHostEnvironment);

            // Enable internet access to wwwroot
            app.UseStaticFiles();

            // Configure HTTP endpoints for controllers and Razor pages
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });
        }
    }
}