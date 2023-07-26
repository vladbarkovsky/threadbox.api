using FluentValidation;

namespace ThreadboxApi.Application.Common.Helpers.Pagination
{
    public class PaginatedQuery
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

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