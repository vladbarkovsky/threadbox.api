using FluentValidation;
using ThreadboxApi.Application.Common.Helpers.Validation;

namespace ThreadboxApi.Dtos
{
    public class LoginFormDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public class LoginFormDtoValidator : AbstractValidator<LoginFormDto>
        {
            public LoginFormDtoValidator()
            {
                RuleFor(x => x.UserName).ValidateUserName();
                RuleFor(x => x.Password).ValidatePassword();
            }
        }
    }

    public class RegistrationFormDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public Guid RegistrationKeyId { get; set; }

        public class RegistrationFormDtoValidator : AbstractValidator<RegistrationFormDto>
        {
            public RegistrationFormDtoValidator()
            {
                RuleFor(x => x.UserName).ValidateUserName();
                RuleFor(x => x.Password).ValidatePassword();
                RuleFor(x => x.ConfirmPassword).Equal(x => x.Password);
                RuleFor(x => x.RegistrationKeyId).NotEmpty();
            }
        }
    }
}