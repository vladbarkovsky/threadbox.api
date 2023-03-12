using FluentValidation;
using ThreadboxApi.Tools;

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
				RuleFor(x => x.UserName).UserNameRules();
				RuleFor(x => x.Password).PasswordRules();
			}
		}
	}

	public class RegistrationFormDto
	{
		public string UserName { get; set; }
		public string Password { get; set; }
		public string ConfirmPassword { get; set; }
		public string RegistrationToken { get; set; }

		public class RegistrationFormDtoValidator : AbstractValidator<RegistrationFormDto>
		{
			public RegistrationFormDtoValidator()
			{
				RuleFor(x => x.UserName).UserNameRules();
				RuleFor(x => x.Password).PasswordRules();
				RuleFor(x => x.ConfirmPassword).Equal(x => x.Password);
				RuleFor(x => x.RegistrationToken).NotEmpty();
			}
		}
	}
}