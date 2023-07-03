using FluentValidation;

namespace ThreadboxApi.Application.Common.Helpers.Pagination
{
    public class PaginatedQuery
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        public class Validator : AbstractValidator<PaginatedQuery>
        {
            public Validator()
            {
                RuleFor(x => x.PageIndex).GreaterThanOrEqualTo(0);
                RuleFor(x => x.PageSize).GreaterThan(0);
            }
        }
    }
}