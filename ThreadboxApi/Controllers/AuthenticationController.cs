using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThreadboxApi.Dtos;
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
		public async Task<ActionResult<string>> Login(LoginFormDto loginFormDto)
		{
			return await _authenticationService.Login(loginFormDto);
		}

		[HttpPost("[action]")]
		public async Task<ActionResult> Register(RegistrationFormDto registrationFormDto)
		{
			await _authenticationService.Register(registrationFormDto);
			return Ok();
		}

		[Authorize]
		[HttpGet("[action]")]
		public async Task<ActionResult<string>> GetRegistrationToken()
		{
			return await _authenticationService.CreateRegistrationTokenAsync();
		}
	}
}