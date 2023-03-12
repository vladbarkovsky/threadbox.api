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
			var accessToken = await _authenticationService.Login(loginFormDto);
			return accessToken;
		}

		[Authorize]
		[HttpGet("[action]")]
		public ActionResult<string> RefreshAccessToken()
		{
			var accessToken = _authenticationService.RefreshAccessToken();
			return accessToken;
		}

		// TODO: Admin access
		[Authorize]
		[HttpGet("[action]")]
		public async Task<ActionResult<string>> CreateRegistrationUrl()
		{
			var registrationUrl = await _authenticationService.CreateRegistrationUrlAsync();
			return registrationUrl;
		}

		[HttpGet("[action]")]
		public async Task<ActionResult> ValidateRegistrationKey(Guid registrationKeyId)
		{
			await _authenticationService.ValidateRegistrationKeyAsync(registrationKeyId);
			return NoContent();
		}

		[HttpPost("[action]")]
		public async Task<ActionResult> Register(RegistrationFormDto registrationFormDto)
		{
			await _authenticationService.Register(registrationFormDto);
			return NoContent();
		}

		[Authorize]
		[HttpGet("[action]")]
		public async Task<ActionResult<string>> CreateRegistrationKey()
		{
			return await _authenticationService.CreateRegistrationUrlAsync();
		}
	}
}