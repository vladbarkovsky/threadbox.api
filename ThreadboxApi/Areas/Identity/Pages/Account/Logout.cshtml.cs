using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ThreadboxApi.Application.Common.Helpers;
using ThreadboxApi.Infrastructure.Identity;

namespace ThreadboxApi.Areas.Identity.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IIdentityServerInteractionService _identityServerInteractionService;

        public LogoutModel(SignInManager<ApplicationUser> signInManager, IIdentityServerInteractionService identityServerInteractionService)
        {
            _signInManager = signInManager;
            _identityServerInteractionService = identityServerInteractionService;
        }

        public async Task<ActionResult> OnGet()
        {
            var logoutId = Request.Query["logoutId"].ToString();

            if (string.IsNullOrEmpty(logoutId))
            {
                throw HttpResponseException.BadRequest;
            }

            var logoutContext = await _identityServerInteractionService.GetLogoutContextAsync(logoutId);
            var returnUrl = logoutContext.PostLogoutRedirectUri;

            if (string.IsNullOrEmpty(returnUrl))
            {
                throw HttpResponseException.BadRequest;
            }

            if (!_signInManager.IsSignedIn(User))
            {
                throw HttpResponseException.BadRequest;
            }

            await _signInManager.SignOutAsync();
            return Redirect(returnUrl);
        }
    }
}