using Microsoft.AspNetCore.Mvc;

namespace ThreadboxApi.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class ConnectionController : ControllerBase
	{
		public ConnectionController(IServiceProvider services)
		{ }

		[HttpGet("[action]")]
		public async Task<ActionResult> Check()
		{
			return Ok();
		}
	}
}