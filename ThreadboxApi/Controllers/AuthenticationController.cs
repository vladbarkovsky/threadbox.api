using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ThreadboxApi.Dtos;
using ThreadboxApi.Models;
using ThreadboxApi.Services;

namespace ThreadboxApi.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class AuthenticationController : ControllerBase
	{
		private readonly AuthenticationService _authenticationService;
		private readonly UserManager<User> _userManager;
		private readonly IMapper _mapper;

		public AuthenticationController(IServiceProvider services)
		{
			_authenticationService = services.GetRequiredService<AuthenticationService>();
			_userManager = services.GetRequiredService<UserManager<User>>();
			_mapper = services.GetRequiredService<IMapper>();
		}

		[HttpPost("[action]")]
		public async Task<ActionResult<string>> Login(LoginFormDto loginFormDto)
		{
			var user = await _userManager.FindByNameAsync(loginFormDto.UserName);

			if (user == null)
			{
				return BadRequest("Can't found user with such name.");
			}

			var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, loginFormDto.Password);

			if (!isPasswordCorrect)
			{
				return BadRequest("Password is incorrect.");
			}

			return _authenticationService.CreateAuthenticationToken(user.Id);
		}

		[HttpPost("[action]")]
		public async Task<ActionResult> Register(RegistrationFormDto registrationFormDto)
		{
			var isTokenAccepted = await _authenticationService.UseRegistrationTokenAsync(registrationFormDto.RegistrationToken);

			if (!isTokenAccepted)
			{
				return BadRequest("Invalid registration token.");
			}

			var user = _mapper.Map<User>(registrationFormDto);
			var identityResult = await _userManager.CreateAsync(user, registrationFormDto.Password);

			return identityResult.Succeeded ? Ok() : BadRequest("Something went wrong during user creation.");
		}

		[Authorize]
		[HttpGet("[action]")]
		public async Task<ActionResult<string>> GetRegistrationToken()
		{
			return await _authenticationService.CreateRegistrationTokenAsync();
		}
	}
}