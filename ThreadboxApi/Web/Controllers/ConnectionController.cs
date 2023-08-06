using Microsoft.AspNetCore.Mvc;

namespace ThreadboxApi.Web.Controllers
{
    public class ConnectionController : MediatRController
    {
        [HttpGet("[action]")]
        public ActionResult CheckConnection()
        {
            return NoContent();
        }
    }
}