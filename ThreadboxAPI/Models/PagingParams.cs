using FluentValidation;

namespace ThreadboxAPI.Models
{
    public class PagingParamsDto
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
    }

    public class PagingParamsDtoValidator : AbstractValidator<PagingParamsDto>
    {
        public PagingParamsDtoValidator()
        {
            RuleFor(x => x.CurrentPage).GreaterThan(0);
            RuleFor(x => x.PageSize).GreaterThan(0);
        }
    }
}
