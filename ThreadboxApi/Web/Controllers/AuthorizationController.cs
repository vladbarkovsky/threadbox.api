using MediatR;
using Microsoft.AspNetCore.Mvc;
using ThreadboxApi.Application.Identity.Commands;
using ThreadboxApi.Web.Constants;

namespace ThreadboxApi.Web.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public sealed class AuthorizationController : ApplicationController
    {
        public AuthorizationController(ISender sender) : base(sender) { }

        [HttpGet(UrlConstants.ConnectAuthorizePath)]
        [HttpPost(UrlConstants.ConnectAuthorizePath)]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Authorize()
        {
            return await _sender.Send(new Authorize.Command());
        }
    }
}
