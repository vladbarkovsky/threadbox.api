using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThreadboxApi.Dtos;
using ThreadboxApi.Infrastructure.Identity;

namespace ThreadboxApi.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IdentityService _identityService;

        public AuthenticationController(IServiceProvider services)
        {
            _identityService = services.GetRequiredService<IdentityService>();
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<string>> Login(LoginFormDto loginFormDto)
        {
            var accessToken = await _identityService.Login(loginFormDto);
            return accessToken;
        }

        [Authorize]
        [HttpGet("[action]")]
        public ActionResult<string> RefreshAccessToken()
        {
            var accessToken = _identityService.RefreshAccessToken();
            return accessToken;
        }

        // TODO: Admin access
        [Authorize]
        [HttpGet("[action]")]
        public async Task<ActionResult<string>> CreateRegistrationUrl()
        {
            var registrationUrl = await _identityService.CreateRegistrationUrlAsync();
            return registrationUrl;
        }

        [HttpGet("[action]")]
        public async Task<ActionResult> ValidateRegistrationKey(Guid registrationKeyId)
        {
            await _identityService.ValidateRegistrationKeyAsync(registrationKeyId);
            return NoContent();
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> Register(RegistrationFormDto registrationFormDto)
        {
            await _identityService.Register(registrationFormDto);
            return NoContent();
        }

        [Authorize]
        [HttpGet("[action]")]
        public async Task<ActionResult<string>> CreateRegistrationKey()
        {
            return await _identityService.CreateRegistrationUrlAsync();
        }
    }
}