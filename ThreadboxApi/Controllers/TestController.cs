using Microsoft.AspNetCore.Mvc;
using ThreadboxApi.Configuration;
using ThreadboxApi.Services;

namespace ThreadboxApi.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class TestController : ControllerBase
	{
		private readonly ThreadboxAppContext _appContext;
		private readonly AuthenticationService _authenticationService;

		public TestController(IServiceProvider services)
		{
			_appContext = services.GetRequiredService<ThreadboxAppContext>();
			_authenticationService = services.GetRequiredService<AuthenticationService>();
		}

		[HttpGet("[action]")]
		public async Task<ActionResult<string>> Check()
		{
			// Testing app context
			//var userId = _appContext.UserId;
			//var user = await _appContext.TryGetUserAsync();
			//var roles = await _appContext.TryGetRolesAsync();

			//return NoContent();

			// Testing registration JWT generation
			return await _authenticationService.CreateRegistrationTokenAsync();
		}
	}
}