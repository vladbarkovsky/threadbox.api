using FluentValidation;
using FluentValidation.AspNetCore;
using System.Reflection;
using ThreadboxApi.Configuration.Startup;

namespace ThreadboxApi
{
    public class Startup
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddFluentValidationAutoValidation();

            SwaggerStartup.ConfigureServices(services);
        }

        public static void Configure(WebApplication app)
        {
            app.MapControllers();

            SwaggerStartup.Configure(app);

            app.UseHttpsRedirection();
        }
    }
}