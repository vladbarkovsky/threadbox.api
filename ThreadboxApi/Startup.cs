using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Reflection;
using ThreadboxApi.Configuration;
using ThreadboxApi.Configuration.Startup;
using ThreadboxApi.Configuraton.Startup;
using ThreadboxApi.Models;

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
			services.AddDbContext<ThreadboxDbContext>(options =>
			{
				options.UseNpgsql(_configuration.GetConnectionString(AppSettings.DbConnectionString));

				// Throw exceptions in case of performance issues with single queries
				// See https://learn.microsoft.com/en-us/ef/core/querying/single-split-queries
				options.ConfigureWarnings(w => w.Throw(RelationalEventId.MultipleCollectionIncludeWarning));
			});

			services.AddIdentity<User, IdentityRole<Guid>>()
				.AddEntityFrameworkStores<ThreadboxDbContext>();

			DependencyInjectionStartup.ConfigureServices(services);

			services.AddControllers();
			SwaggerStartup.ConfigureServices(services);
			AntiForgeryStartup.ConfigureServices(services);
			CorsStartup.ConfigureServices(services, _configuration);
			ExceptionHandlingStartup.ConfigureServices(services);

			AuthenticationStartup.ConfigureServices(services, _configuration);
			services.AddAuthorization();

			services.AddAutoMapper(Assembly.GetExecutingAssembly());

			services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
			services.AddFluentValidationAutoValidation();
		}

		public void Configure(IApplicationBuilder app)
		{
			SwaggerStartup.Configure(app, _webHostEnvironment);
			AntiForgeryStartup.Configure(app);

			/// IMPORTANT: CORS must be configured before
			/// <see cref="ControllerEndpointRouteBuilderExtensions.MapControllers"/>,
			/// <see cref="AuthorizationAppBuilderExtensions.UseAuthorization"/>,
			/// <see cref="HttpsPolicyBuilderExtensions.UseHttpsRedirection"/>,

			CorsStartup.Configure(app);
			ExceptionHandlingStartup.Configure(app);

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});

			app.UseHttpsRedirection();
		}
	}
}