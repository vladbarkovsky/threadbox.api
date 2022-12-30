using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using ThreadboxApi.Configuration;
using ThreadboxApi.Configuration.Startup;
using ThreadboxApi.Configuraton.Startup;
using ThreadboxApi.Models;
using ThreadboxApi.Services;

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

			services.AddIdentity<User, IdentityRole<Guid>>()
				.AddEntityFrameworkStores<ThreadboxDbContext>();

			DependencyInjectionStartup.ConfigureServices(services);

			services.AddControllers();
			SwaggerStartup.ConfigureServices(services);
			CorsStartup.ConfigureServices(services, configuration);

			AuthenticationStartup.ConfigureServices(services, configuration);
			services.AddAuthorization();

			services.AddAutoMapper(Assembly.GetExecutingAssembly());

			services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
			services.AddFluentValidationAutoValidation();
		}

		public static async Task ConfigureAsync(WebApplication app)
		{
			using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
			{
				await scope.ServiceProvider.GetRequiredService<DbSeedingService>().SeedDbAsync();
			}

			SwaggerStartup.Configure(app);

			/// IMPORTANT: CORS must be configured before
			/// <see cref="ControllerEndpointRouteBuilderExtensions.MapControllers"/>,
			/// <see cref="HttpsPolicyBuilderExtensions.UseHttpsRedirection"/>
			/// <see cref="AuthorizationAppBuilderExtensions.UseAuthorization"/>),
			/// otherwise it will cause HTTP responses with status 0
			CorsStartup.Configure(app);

			app.MapControllers();
			app.UseHttpsRedirection();

			app.UseAuthentication();
			app.UseAuthorization();
		}
	}
}