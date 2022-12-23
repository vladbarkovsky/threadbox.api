using Microsoft.AspNetCore.Identity;
using ThreadboxApi.Configuration.Startup;

namespace ThreadboxApi.Services
{
	public class DbSeedingService : ITransientService
	{
		private readonly UserManager<IdentityUser<Guid>> _userManager;

		public DbSeedingService(IServiceProvider services)
		{
			_userManager = services.GetRequiredService<UserManager<IdentityUser<Guid>>>();
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

			await _userManager.CreateAsync(new IdentityUser<Guid>("admin"), "P@ssw0rd");
		}
	}
}