﻿using FluentValidation;
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

        public static IRuleBuilderOptions<T, IEnumerable<TObject>> WithUnique<T, TObject>(
            this IRuleBuilder<T, IEnumerable<TObject>> ruleBuilder,
            Func<TObject, object> selector)
        {
            return ruleBuilder
                .Must((rootObject, collection, context) =>
                {
                    if (collection == null)
                    {
                        return true;
                    }

                    var duplicates = collection
                        .GroupBy(selector)
                        .Where(group => group.Count() > 1)
                        .SelectMany(group => group.Skip(1))
                        .ToList();

                    if (duplicates.Any())
                    {
                        foreach (var duplicate in duplicates)
                        {
                            context.AddFailure($"Element '{duplicate}' is not unique.");
                        }

                        return false;
                    }

                    return true;
                })
                .WithMessage("The collection contains duplicate elements.");
        }
    }
}