namespace ThreadboxApi
{
	public class Program
	{
		private static async Task Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
			Startup.ConfigureServices(builder.Services, builder.Configuration);
			var app = builder.Build();
			await Startup.ConfigureAsync(app);
			app.Run();
		}
	}
}