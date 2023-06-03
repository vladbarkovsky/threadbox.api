using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ThreadboxApi.Configuration.Startup
{
    public class DbStartup
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ThreadboxDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString(AppSettings.DbConnectionString));

                // Throw exceptions in case of performance issues with single queries
                // See https://learn.microsoft.com/en-us/ef/core/querying/single-split-queries
                options.ConfigureWarnings(w => w.Throw(RelationalEventId.MultipleCollectionIncludeWarning));
            });
        }
    }
}