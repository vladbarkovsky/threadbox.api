using FluentValidation;
using ThreadboxApi.Application.Common.Helpers.Validation;

namespace ThreadboxApi.Web.ViewModels.Account
{
    public class LoginInputModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool RememberLogin { get; set; }
        public string ReturnUrl { get; set; }

        public class Validator : AbstractValidator<LoginInputModel>
        {
            public Validator()
            {
                RuleFor(x => x.Username).ValidateUserName();
                RuleFor(x => x.Password).ValidatePassword();
            }
        }
    }
}