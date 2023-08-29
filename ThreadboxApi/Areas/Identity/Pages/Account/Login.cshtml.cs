﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using ThreadboxApi.Application.Common.Helpers;
using ThreadboxApi.Infrastructure.Identity;

namespace ThreadboxApi.Areas.Identity.Pages.Account
{
    public class LoginPageModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        public LoginPageModel(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }

        [BindProperty]
        [Required]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [MinLength(5)]
            [MaxLength(20)]
            public string UserName { get; set; }

            [Required]
            [MinLength(8)]
            [MaxLength(30)]
            public string Password { get; set; }

            [Required]
            public bool OfflineAccess { get; set; }
        }

        public void OnGet([FromQuery] string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl))
            {
                throw HttpResponseException.BadRequest;
            }

            if (_signInManager.IsSignedIn(User))
            {
                throw HttpResponseException.BadRequest;
            }
        }

        public async Task<ActionResult> OnPostAsync([FromQuery] string returnUrl)
        {
            var result = await _signInManager.PasswordSignInAsync(
                userName: Input.UserName,
                password: Input.Password,
                isPersistent: Input.OfflineAccess,
                lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Invalid user name or password.");
                return Page();
            }

            return LocalRedirect(returnUrl);
        }
    }
}
