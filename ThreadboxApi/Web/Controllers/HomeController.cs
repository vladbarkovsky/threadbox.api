using IdentityServer4.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThreadboxApi.Web.IdentityServer;
using ThreadboxApi.Web.ViewModels.Home;

namespace ThreadboxApi.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [SecurityHeaders]
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IWebHostEnvironment _environment;

        public HomeController(IIdentityServerInteractionService interaction, IWebHostEnvironment environment, ILogger<HomeController> logger)
        {
            _interaction = interaction;
            _environment = environment;
        }

        /// <summary>
        /// Shows the error page
        /// </summary>
        [HttpGet("[action]")]
        public async Task<IActionResult> Error([FromQuery] string errorId)
        {
            var vm = new ErrorViewModel();

            // retrieve error details from identityserver
            var message = await _interaction.GetErrorContextAsync(errorId);
            if (message != null)
            {
                vm.Error = message;

                if (!_environment.IsDevelopment())
                {
                    // Show error description only in development
                    message.ErrorDescription = null;
                }
            }

            return View(vm);
        }
    }
}