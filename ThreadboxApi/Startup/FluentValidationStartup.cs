using FluentValidation.AspNetCore;
using FluentValidation;
using System.Reflection;

namespace ThreadboxApi.Startup
{
    public class FluentValidationStartup
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddFluentValidationAutoValidation();
        }
    }
}