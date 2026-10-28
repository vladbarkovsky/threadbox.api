using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThreadboxApi.Application.Identity.Models;
using ThreadboxApi.Application.Identity.Queries;

namespace ThreadboxApi.Web.Controllers
{
    [Route("api/user-session")]
    public sealed class UserSessionController : ApplicationController
    {
        public UserSessionController(ISender sender) : base(sender) { }

        [HttpGet]
        [Authorize]
        public async Task<UserSession> Get()
        {
            return await _sender.Send(new GetUserSession.Query());
        }
    }
}
