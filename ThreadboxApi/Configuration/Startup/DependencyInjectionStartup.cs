namespace ThreadboxApi.Configuration.Startup
{
	public interface ITransientService
	{ }

	public interface IScopedService
	{ }

	public interface ISingletonService
	{ }

	public class DependencyInjectionStartup
	{
		public static void ConfigureServices(IServiceCollection services)
		{
			services.Scan(scan => scan.FromAssemblyOf<Program>()
				.AddClasses(x => x.AssignableTo<ISingletonService>())
					.AsSelfWithInterfaces()
					.WithSingletonLifetime()
				.AddClasses(x => x.AssignableTo<IScopedService>())
					.AsSelfWithInterfaces()
					.WithScopedLifetime()
				.AddClasses(x => x.AssignableTo<ITransientService>())
					.AsSelfWithInterfaces()
					.WithTransientLifetime());

			services.AddHttpContextAccessor();
		}
	}
}