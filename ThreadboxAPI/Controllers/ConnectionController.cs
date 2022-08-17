using Microsoft.AspNetCore.Mvc;

namespace ThreadboxAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConnectionController : ControllerBase
    {
        [HttpGet("[action]")]
        public ActionResult Check()
        {
            return Ok();
        }
    }
}
