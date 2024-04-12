using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ThreadboxApi.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class MediatRController : ControllerBase
    {
        protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetService<ISender>();
        private ISender _mediator;
    }
}