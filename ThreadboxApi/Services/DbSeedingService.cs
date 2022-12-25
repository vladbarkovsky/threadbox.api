using Microsoft.AspNetCore.Identity;
using ThreadboxApi.Configuration.Startup;
using ThreadboxApi.Models;

namespace ThreadboxApi.Services
{
	public class DbSeedingService : ITransientService
	{
		private readonly UserManager<User> _userManager;

		public DbSeedingService(IServiceProvider services)
		{
			_userManager = services.GetRequiredService<UserManager<User>>();
		}

		public async Task SeedDbAsync()
		{
			await SeedUsersAsync();
		}

		public async Task SeedUsersAsync()
		{
			if (_userManager.Users.Any())
			{
				return;
			}

			await _userManager.CreateAsync(new User("admin"), "P@ssw0rd");
		}
	}
}