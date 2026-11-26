using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using ThreadboxApi.Application.Bff.Commands;
using ThreadboxApi.Configuration;
using ThreadboxApi.ORM.Entities;
using ThreadboxApi.Web.ErrorHandling;

namespace ThreadboxApi.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly IOptionsMonitor<AppSettings> _appSettings;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IStringLocalizer<LoginModel> _localizer;

        public LoginModel(
            IOptionsMonitor<AppSettings> appSettings,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IStringLocalizer<LoginModel> localizer)
        {
            _appSettings = appSettings;
            _signInManager = signInManager;
            _userManager = userManager;
            _localizer = localizer;
        }

        [BindProperty]
        [Required]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Login_FieldRequired")]
            [Display(Name = "Login_Username")]
            public string UserName { get; set; }

            [Required(ErrorMessage = "Login_FieldRequired")]
            [Display(Name = "Login_Password")]
            public string Password { get; set; }

            [Required]
            [Display(Name = "Login_RememberMe")]
            public bool OfflineAccess { get; set; }
        }

        public IActionResult OnGet([FromQuery] string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl))
            {
                throw new HttpResponseException("Return URL not specified.");
            }

            if (_signInManager.IsSignedIn(User))
            {
                return LocalRedirect(returnUrl);
            }

            return Page();
        }

        public async Task<ActionResult> OnPostAsync([FromQuery] string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl))
            {
                throw new HttpResponseException("Return URL not specified.");
            }

            if (_signInManager.IsSignedIn(User))
            {
                return LocalRedirect(returnUrl);
            }

            var result = await _signInManager.PasswordSignInAsync(
                userName: Input.UserName,
                password: Input.Password,
                isPersistent: Input.OfflineAccess,
                lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, _localizer["Login_InvalidCredentials"]);
                return Page();
            }

            ApplicationUser user = await _userManager.FindByNameAsync(Input.UserName);

            int sessionLifetimeSeconds = Input.OfflineAccess
                ? _appSettings.CurrentValue.AbsoluteRefreshTokenLifetimeSeconds
                : _appSettings.CurrentValue.AccessTokenLifetimeSeconds;

            await _signInManager.SignInAsync(
                user,
                new AuthenticationProperties
                {
                    AllowRefresh = false,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddSeconds(sessionLifetimeSeconds),
                    IsPersistent = Input.OfflineAccess
                });

            Response.Cookies.Append(
                ContinueLogin.OfflineAccessCookieName,
                Input.OfflineAccess ? "true" : "false",
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddMinutes(5)
                });

            return Redirect(returnUrl);
        }
    }
}