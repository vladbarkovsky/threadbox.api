using FluentValidation;

namespace ThreadboxApi.Application.Common.Pagination
{
    public abstract class PaginatedQueryValidatorTemplate<TQuery> : AbstractValidator<TQuery>
        where TQuery : PaginatedQuery
    {
        public PaginatedQueryValidatorTemplate()
        {
            RuleFor(x => x.PageIndex).GreaterThanOrEqualTo(0);
            RuleFor(x => x.PageSize).GreaterThan(0);

            Configure();
        }

        protected virtual void Configure() { }
    }
}