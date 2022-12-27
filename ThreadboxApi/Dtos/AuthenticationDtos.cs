using FluentValidation;
using ThreadboxApi.Tools;

namespace ThreadboxApi.Dtos
{
	public class LoginFormDto
	{
		public string UserName { get; set; } = null!;
		public string Password { get; set; } = null!;

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
		public string UserName { get; set; } = null!;
		public string Password { get; set; } = null!;
		public string ConfirmPassword { get; set; } = null!;
		public string RegistrationToken { get; set; } = null!;

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