using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using ThreadboxApi.Services;

namespace ThreadboxApi.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class AuthenticationController : ControllerBase
	{
		private readonly AuthenticationService _authenticationService;

		public AuthenticationController(IServiceProvider services)
		{
			_authenticationService = services.GetRequiredService<AuthenticationService>();
		}

		[HttpPost("[action]")]
		public async Task<ActionResult<string>> Login()
		{
			return string.Empty;
		}
	}
}