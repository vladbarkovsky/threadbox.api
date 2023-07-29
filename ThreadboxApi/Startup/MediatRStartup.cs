using System.Reflection;

namespace ThreadboxApi.Startup
{
    public class MediatRStartup
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddMediatR(configuration =>
            {
                configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });
        }
    }
}