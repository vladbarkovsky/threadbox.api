using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Reflection;
using ThreadboxApi.Application.Common;
using ThreadboxApi.Application.Common.Constants;
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
            var appSettings = _configuration.Get<AppSettings>();

            LocalizationStartup.ConfigureServices(services);
            ErrorHandlingStartup.ConfigureServices(services);

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(appSettings.ConnectionStrings.Postgres);

                // Throw exceptions in case of performance issues with single queries.
                // See https://learn.microsoft.com/en-us/ef/core/querying/single-split-queries.
                options.ConfigureWarnings(w => w.Throw(RelationalEventId.MultipleCollectionIncludeWarning));
            });

            services.AddHealthChecks().AddDbContextCheck<ApplicationDbContext>();

            services.Scan(scan => scan.FromAssemblyOf<Program>()
                .AddClasses(x => x.AssignableTo<ISingletonService>())
                .AsSelfWithInterfaces()
                .WithSingletonLifetime()
                .AddClasses(x => x.AssignableTo<IScopedService>())
                .AsSelfWithInterfaces()
                .WithScopedLifetime()
                .AddClasses(x => x.AssignableTo<ITransientService>())
                .AsSelfWithInterfaces()
                .WithTransientLifetime());

            services.AddHttpContextAccessor();
            SecurityStartup.ConfigureServices(services, appSettings, _webHostEnvironment);

            services.AddControllers(options =>
            {
                var jsonInputFormatter = options.InputFormatters
                    .OfType<SystemTextJsonInputFormatter>()
                    .Single();

                jsonInputFormatter.SupportedMediaTypes.Add("application/csp-report");
            });

            NSwagStartup.ConfigureServices(services, _webHostEnvironment);
            IdentityStartup.ConfigureServices(services, appSettings, _webHostEnvironment);

            services.AddMediatR(configuration =>
            {
                configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });

            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddFluentValidationAutoValidation();

            if (_webHostEnvironment.IsDevelopment())
            {
                services.AddDatabaseDeveloperPageExceptionFilter();
            }

            if (!_webHostEnvironment.IsDevelopment())
            {
                services.Configure<ForwardedHeadersOptions>(options =>
                {
                    options.ForwardedHeaders =
                        ForwardedHeaders.XForwardedHost |
                        ForwardedHeaders.XForwardedProto |
                        ForwardedHeaders.XForwardedFor;
                });
            }
        }

        public void Configure(IApplicationBuilder app)
        {
            var appSettings = _configuration.Get<AppSettings>();

            LocalizationStartup.Configure(app);
            app.UseMiddleware<TraceIdLoggingMidleware>();
            ErrorHandlingStartup.Configure(app);
            SecurityStartup.Configure(app, appSettings, _webHostEnvironment);

            if (_webHostEnvironment.IsProduction())
            {
                app.UseForwardedHeaders();
                app.UsePathBase("/threadbox-api");
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });

            app.UseHealthChecks("/health");
            IdentityStartup.Configure(app);
            app.UseStaticFiles();

            if (_webHostEnvironment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            NSwagStartup.Configure(app, _webHostEnvironment);
        }
    }
}