using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ThreadboxApi.Web.Controllers
{
    [ApiController]
    public abstract class ApplicationController : ControllerBase
    {
        protected readonly ISender _sender;

        public ApplicationController(ISender sender)
        {
            _sender = sender;
        }
    }
}