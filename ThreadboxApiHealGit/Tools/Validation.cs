using FluentValidation;

namespace ThreadboxApi.Tools
{
	public static class Validation
	{
		public static IRuleBuilderOptions<T, string> UserNameRules<T>(this IRuleBuilder<T, string> builder)
		{
			return builder
				.NotEmpty()
				.MinimumLength(5)
				.MaximumLength(20);
		}

		public static IRuleBuilderOptions<T, string> PasswordRules<T>(this IRuleBuilder<T, string> builder)
		{
			return builder
				.NotEmpty()
				.MinimumLength(8)
				.MaximumLength(30);
		}
	}
}