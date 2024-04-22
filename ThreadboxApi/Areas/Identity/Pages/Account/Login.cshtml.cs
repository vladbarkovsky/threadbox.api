﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ThreadboxApi.ORM.Entities;
using ThreadboxApi.Web;

namespace ThreadboxApi.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        public LoginModel(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }

        [BindProperty]
        [Required]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [DisplayName("User name")]
            public string UserName { get; set; }

            [Required]
            [DisplayName("Password")]
            public string Password { get; set; }

            [Required]
            [DisplayName("Remember me")]
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
            if (string.IsNullOrEmpty(returnUrl))
            {
                throw HttpResponseException.BadRequest;
            }

            if (_signInManager.IsSignedIn(User))
            {
                throw HttpResponseException.BadRequest;
            }

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

            return Redirect(returnUrl);
        }
    }
}