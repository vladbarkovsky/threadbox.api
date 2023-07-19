using FluentValidation;
using ThreadboxApi.Application.Common.Constants;

namespace ThreadboxApi.Application.Common.Helpers.Validation
{
    public static class CustomValidators
    {
        public static IRuleBuilderOptions<T, string> ValidateUserName<T>(this IRuleBuilder<T, string> builder)
        {
            return builder
                .NotEmpty()
                .MinimumLength(5)
                .MaximumLength(20);
        }

        public static IRuleBuilderOptions<T, string> ValidatePassword<T>(this IRuleBuilder<T, string> builder)
        {
            return builder
                .NotEmpty()
                .MinimumLength(8)
                .MaximumLength(30);
        }

        public static IRuleBuilderOptions<T, IFormFile> ValidateImage<T>(this IRuleBuilder<T, IFormFile> builder)
        {
            return builder
                .NotEmpty()
                .Must(x => x.Length > 0)
                .WithMessage("Empty file.")
                .Must(x => x.Length <= 10 * 1024 * 1024)
                .WithMessage("Maximal allowed file size is 10 MB.")
                .Must(file => MediaConstants.AllowedImages.Any(allowedImage =>
                    file.ContentType == allowedImage.ContentType &&
                    Path.GetExtension(file.Name) == allowedImage.Extension))
                .WithMessage("File format is not allowed or ContentType is not compatible with extension.");
        }
    }
}