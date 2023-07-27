using FluentValidation;

namespace ThreadboxApi.Application.Common.Helpers.Pagination
{
    public class PaginatedQueryValidator<T> : AbstractValidator<T>
        where T : PaginatedQuery
    {
        public PaginatedQueryValidator()
        {
            RuleFor(x => x.PageIndex).GreaterThanOrEqualTo(0);
            RuleFor(x => x.PageSize).GreaterThan(0);
        }
    }
}