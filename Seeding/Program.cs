using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Seeding.Seeders;
using ThreadboxApi;
using ThreadboxApi.Application.Files;
using ThreadboxApi.Configuration;
using ThreadboxApi.Configuration.Identity;
using ThreadboxApi.ORM;
using ThreadboxApi.ORM.Entities;
using ThreadboxApi.Time;

namespace Seeding
{
    public class Program
    {
        public static async Task<int> Main(string[] args)
        {
            using IHost host = Host
                .CreateDefaultBuilder(args)
                .UseContentRoot(AppContext.BaseDirectory)
                .ConfigureAppConfiguration(void (HostBuilderContext hostBuilderContext, IConfigurationBuilder configurationBuilder) =>
                {
                    configurationBuilder.Sources.Clear();
                    configurationBuilder.SetBasePath(AppContext.BaseDirectory);
                    configurationBuilder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);
                    configurationBuilder.AddUserSecrets<Program>();
                    configurationBuilder.AddEnvironmentVariables();
                    configurationBuilder.AddCommandLine(args);
                })
                .ConfigureServices(void (HostBuilderContext hostBuilderContext, IServiceCollection services) =>
                {
                    services.Configure<AppSettings>(hostBuilderContext.Configuration);

                    var appSettings = new AppSettings();
                    hostBuilderContext.Configuration.Bind(appSettings);

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

                    services.AddOpenIddict()
                        .AddCore(void (OpenIddictCoreBuilder builder) =>
                        {
                            builder.UseEntityFrameworkCore().UseDbContext<ApplicationDbContext>();
                        });

                    IdentityPasswordConfiguration.Configure(services);

                    services
                        .AddScoped<IDateTimeService, DateTimeService>()
                        .AddHttpContextAccessor()
                        .AddScoped<ApplicationContext>()
                        .AddScoped<IFileStorage, DbFileStorage>()
                        .AddScoped<OpenIddictApplicationSeeder>()
                        .AddScoped<RoleSeeder>()
                        .AddScoped<UserSeeder>()
                        .AddScoped<SectionSeeder>()
                        .AddScoped<BoardSeeder>()
                        .AddScoped<ThreadSeeder>()
                        .AddScoped<ThreadImageSeeder>()
                        .AddScoped<PostSeeder>()
                        .AddScoped<PostImageSeeder>()
                        .AddScoped<SeedingService>();
                })
                .Build();

            using (IServiceScope scope = host.Services.CreateScope())
            {
                SeedingService seedingService = scope.ServiceProvider.GetRequiredService<SeedingService>();
                await seedingService.SeedAsync();
            }

            Console.WriteLine("Seeding completed.");
            return 0;
        }
    }
}
