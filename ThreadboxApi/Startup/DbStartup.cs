using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ThreadboxApi.Application.Common;
using ThreadboxApi.Infrastructure.Persistence;

namespace ThreadboxApi.Startup
{
    public class DbStartup
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<Infrastructure.Persistence.AppDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString(AppSettings.DevDbConnectionString));

                // Throw exceptions in case of performance issues with single queries
                // See https://learn.microsoft.com/en-us/ef/core/querying/single-split-queries
                options.ConfigureWarnings(w => w.Throw(RelationalEventId.MultipleCollectionIncludeWarning));
            });
        }
    }
}