using Microsoft.AspNetCore.Identity;
using ThreadboxApi.Configuration.Startup;
using ThreadboxApi.Models;

namespace ThreadboxApi.Configuration
{
	public class ThreadboxAppContext : IScopedService
	{
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly UserManager<User> _userManager;

		public ThreadboxAppContext(IServiceProvider services)
		{
			_httpContextAccessor = services.GetRequiredService<IHttpContextAccessor>();
			_userManager = services.GetRequiredService<UserManager<User>>();
		}

		public Guid? UserId
		{
			get
			{
				try
				{
					var id = _httpContextAccessor.HttpContext?.User.Claims.First(x => x.Type == ClaimTypes.UserId).Value;
					return Guid.Parse(id!);
				}
				catch
				{
					return null;
				}
			}
		}

		public async Task<User?> TryGetUserAsync()
		{
			try
			{
				return await _userManager.FindByIdAsync(UserId.ToString());
			}
			catch
			{
				return null;
			}
		}

		public async Task<List<string>?> TryGetRolesAsync()
		{
			var user = await TryGetUserAsync();

			try
			{
				var roles = await _userManager.GetRolesAsync(user!);
				return roles.ToList();
			}
			catch
			{
				return null;
			}
		}
	}
}