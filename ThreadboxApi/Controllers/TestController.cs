using Microsoft.AspNetCore.Mvc;
using ThreadboxApi.Configuration;

namespace ThreadboxApi.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class TestController : ControllerBase
	{
		private readonly ThreadboxAppContext _appContext;

		public TestController(IServiceProvider services)
		{
			_appContext = services.GetRequiredService<ThreadboxAppContext>();
		}

		[HttpGet("[action]")]
		public async Task<ActionResult> Check()
		{
			var userId = _appContext.UserId;
			var user = await _appContext.TryGetUserAsync();
			var roles = await _appContext.TryGetRolesAsync();

			return NoContent();
		}
	}
}