using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Scrutor;
using System.Reflection;
using ThreadboxApi.Application.Common;
using ThreadboxApi.Application.Common.Constants;
using ThreadboxApi.Application.Common.Mapping;
using ThreadboxApi.Configuration;
using ThreadboxApi.ORM.Services;
using ThreadboxApi.Web;

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
            services.Configure<AppSettings>(_configuration);
            AppSettings appSettings = _configuration.Get<AppSettings>();

            LocalizationStartup.ConfigureServices(services);
            ErrorHandlingStartup.ConfigureServices(services);

            services.AddDbContext<ApplicationDbContext>(void (DbContextOptionsBuilder optionsBuilder) =>
            {
                optionsBuilder
                    .UseNpgsql(appSettings.ConnectionStrings.Postgres)
                    .ConfigureWarnings(void (WarningsConfigurationBuilder configurationBuilder) =>
                    {
                        configurationBuilder.Throw(RelationalEventId.MultipleCollectionIncludeWarning);
                    });
            });

            services
                .AddHealthChecks()
                .AddDbContextCheck<ApplicationDbContext>();

            services.Scan(void (ITypeSourceSelector selector) =>
            {
                selector
                    .FromAssemblyOf<Program>()
                    .AddClasses(void (IImplementationTypeFilter filter) => filter.AssignableTo<ISingletonService>())
                    .AsSelfWithInterfaces()
                    .WithSingletonLifetime()
                    .AddClasses(void (IImplementationTypeFilter filter) => filter.AssignableTo<IScopedService>())
                    .AsSelfWithInterfaces()
                    .WithScopedLifetime()
                    .AddClasses(void (IImplementationTypeFilter filter) => filter.AssignableTo<ITransientService>())
                    .AsSelfWithInterfaces()
                    .WithTransientLifetime();
            });

            services
                .AddHttpContextAccessor()
                .AddHttpClient();

            SecurityStartup.ConfigureServices(services, appSettings, _webHostEnvironment);

            services.AddControllers(options =>
            {
                var jsonInputFormatter = options.InputFormatters
                    .OfType<SystemTextJsonInputFormatter>()
                    .Single();

                jsonInputFormatter.SupportedMediaTypes.Add("application/csp-report");
            });

            services.AddRazorPages();

            NSwagStartup.ConfigureServices(services, _webHostEnvironment);
            IdentityStartup.ConfigureServices(services, appSettings, _webHostEnvironment);

            services
                .AddMediatR(void (MediatRServiceConfiguration configuration) =>
                {
                    configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                })
                .AddAutoMapper(Assembly.GetExecutingAssembly())
                .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())
                .AddFluentValidationAutoValidation();

            if (_webHostEnvironment.IsDevelopment())
            {
                services.AddDatabaseDeveloperPageExceptionFilter();
            }

            if (!_webHostEnvironment.IsDevelopment())
            {
                services.Configure<ForwardedHeadersOptions>(void (ForwardedHeadersOptions options) =>
                {
                    options.ForwardedHeaders =
                        ForwardedHeaders.XForwardedHost |
                        ForwardedHeaders.XForwardedProto |
                        ForwardedHeaders.XForwardedFor;

                    options.KnownNetworks.Clear();
                    options.KnownProxies.Clear();
                });
            }
        }

        public void Configure(IApplicationBuilder app)
        {
            if (!_webHostEnvironment.IsDevelopment())
            {
                app.UseForwardedHeaders();
            }

            app.UseMiddleware<TraceIdLoggingMidleware>();
            LocalizationStartup.Configure(app);
            ErrorHandlingStartup.Configure(app);
            app.UseHealthChecks("/health");
            SecurityStartup.Configure(app, _webHostEnvironment);
            app.UseStaticFiles();
            app.UseRouting();
            SecurityStartup.ConfigureCors(app);
            ErrorHandlingStartup.Configure(app);
            IdentityStartup.Configure(app);
            NSwagStartup.Configure(app, _webHostEnvironment);

            app.UseEndpoints(void (IEndpointRouteBuilder routeBuilder) =>
            {
                routeBuilder.MapGet("/", () => Results.Redirect("/api")).ExcludeFromDescription();
                routeBuilder.MapControllers();
                routeBuilder.MapRazorPages();
            });
        }
    }
}
