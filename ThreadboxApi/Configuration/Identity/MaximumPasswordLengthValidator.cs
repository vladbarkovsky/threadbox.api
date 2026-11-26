using Microsoft.AspNetCore.Identity;
using ThreadboxApi.ORM.Entities;

namespace ThreadboxApi.Configuration.Identity
{
    public class MaximumPasswordLengthValidator : IPasswordValidator<ApplicationUser>
    {

        public Task<IdentityResult> ValidateAsync(
            UserManager<ApplicationUser> userManager,
            ApplicationUser user,
            string password)
        {
            if (password?.Length > PasswordConstants.MaximumLength)
            {
                IdentityError error = new IdentityError
                {
                    Code = "PasswordTooLong",
                    Description = $"Passwords must be at most {PasswordConstants.MaximumLength} characters."
                };

                return Task.FromResult(IdentityResult.Failed(error));
            }

            return Task.FromResult(IdentityResult.Success);
        }

    }
}
