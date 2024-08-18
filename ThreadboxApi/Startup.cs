using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Reflection;
using ThreadboxApi.Application.Common;
using ThreadboxApi.Application.Common.Constants;
using ThreadboxApi.ORM.Entities;
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
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(_configuration.GetConnectionString(AppSettings.ConnectionStrings.Database));

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
            SecurityStartup.ConfigureServices(services, _configuration, _webHostEnvironment);
            NSwagStartup.ConfigureServices(services, _webHostEnvironment);
            IdentityStartup.ConfigureServices(services, _configuration, _webHostEnvironment);

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
        }

        public void Configure(IApplicationBuilder app)
        {
            SecurityStartup.Configure(app, _webHostEnvironment);
            NSwagStartup.Configure(app, _webHostEnvironment);
            app.UseMiddleware<ProblemMiddleware>();
            app.UseRouting();
            app.UseHealthChecks("/health");
            IdentityStartup.Configure(app);
            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });

            if (_webHostEnvironment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
        }
    }
}