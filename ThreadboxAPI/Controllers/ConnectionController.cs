using Microsoft.AspNetCore.Mvc;
using ThreadboxApi.Services;

namespace ThreadboxAPI.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class ConnectionController : ControllerBase
	{
		private readonly RegistrationService _registrationService;

		public ConnectionController(IServiceProvider services)
		{
			_registrationService = services.GetRequiredService<RegistrationService>();
		}

		[HttpGet("[action]")]
		public async Task<ActionResult> Check()
		{
			var token = await _registrationService.CreateRegistrationTokenAsync();
			//var ok = await _registrationService.CheckRegistrationTokenAsync(token);

			return Ok();
		}
	}
}