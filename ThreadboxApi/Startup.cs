using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Mochieve3.API.Configuration.Startup;
using System.Reflection;
using ThreadboxApi.Configuration;
using ThreadboxApi.Configuration.Startup;

namespace ThreadboxApi
{
    public class Startup
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ThreadboxDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString(AppSettings.DbConnectionString));
            });

            services.AddControllers();

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddFluentValidationAutoValidation();

            CorsStartup.ConfigureServices(services, configuration);
            SwaggerStartup.ConfigureServices(services);
            DependencyInjectionStartup.ConfigureServices(services);
        }

        public static void Configure(WebApplication app)
        {
            app.MapControllers();
            app.UseHttpsRedirection();

            CorsStartup.Configure(app);
            SwaggerStartup.Configure(app);
        }
    }
}