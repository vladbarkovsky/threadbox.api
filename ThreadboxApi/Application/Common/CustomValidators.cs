using FluentValidation;
using ThreadboxApi.Application.Common.Constants;

namespace ThreadboxApi.Application.Common
{
    public static class CustomValidators
    {
        public static IRuleBuilderOptions<T, IFormFile> ValidateImage<T>(this IRuleBuilder<T, IFormFile> builder)
        {
            return builder
                .NotEmpty()
                .Must(x => x.Length > 0)
                .WithMessage("Empty file.")
                .Must(x => x.Length <= 10 * 1024 * 1024)
                .WithMessage(x => "Maximal allowed file size is 10 MB.")
                .Must(file => MediaConstants.AllowedImageFormats.Contains(file.ContentType))
                .WithMessage("Content-Type is not allowed.");
        }
    }
}