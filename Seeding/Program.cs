using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using ThreadboxApi.Application.Common.Constants;
using ThreadboxApi.Application.Services;
using ThreadboxApi.Application.Services.Interfaces;
using ThreadboxApi.ORM.Entities;
using ThreadboxApi.ORM.Services;

namespace Seeding
{
    public class Program
    {
        public static async Task<int> Main(string[] args)
        {
            using IHost host = Host
                .CreateDefaultBuilder(args)
                .UseContentRoot(AppContext.BaseDirectory)
                .ConfigureAppConfiguration((hostBuilderContext, configurationBuilder) =>
                {
                    configurationBuilder.Sources.Clear();
                    configurationBuilder.SetBasePath(AppContext.BaseDirectory);
                    configurationBuilder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);
                    configurationBuilder.AddUserSecrets<Program>();
                    configurationBuilder.AddEnvironmentVariables();
                    configurationBuilder.AddCommandLine(args);
                })
                .ConfigureServices((hostBuilderContext, services) =>
                {
                    services.Configure<AppSettings>(hostBuilderContext.Configuration);

                    var appSettings = new AppSettings();
                    hostBuilderContext.Configuration.Bind(appSettings);

                    services.AddSingleton<IOptions<OperationalStoreOptions>>(Options.Create(new OperationalStoreOptions()));

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
                        .AddIdentity<ApplicationUser, IdentityRole>()
                        .AddEntityFrameworkStores<ApplicationDbContext>();

                    services
                        .AddScoped<IDateTimeService, DateTimeService>()
                        .AddHttpContextAccessor()
                        .AddScoped<ApplicationContext>()
                        .AddScoped<IFileStorage, DbFileStorage>()
                        .AddScoped<SeedingService>();
                })
                .Build();

            using (IServiceScope scope = host.Services.CreateScope())
            {
                SeedingService databaseSeeder = scope.ServiceProvider.GetRequiredService<SeedingService>();
                await databaseSeeder.SeedAsync();
            }

            Console.WriteLine("Seeding completed.");
            return 0;
        }
    }
}
