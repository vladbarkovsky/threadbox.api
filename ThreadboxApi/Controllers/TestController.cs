using Microsoft.AspNetCore.Mvc;

namespace ThreadboxApi.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class TestController : ControllerBase
	{
		public TestController()
		{ }

		[HttpGet("[action]")]
		public async Task<ActionResult<object>> Check()
		{
			return NoContent();
		}
	}
}