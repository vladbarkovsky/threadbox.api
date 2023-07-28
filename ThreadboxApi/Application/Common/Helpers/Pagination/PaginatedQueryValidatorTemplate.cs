using FluentValidation;

namespace ThreadboxApi.Application.Common.Helpers.Pagination
{
    public class PaginatedQueryValidatorTemplate<T> : AbstractValidator<T>
        where T : PaginatedQuery
    {
        public PaginatedQueryValidatorTemplate()
        {
            RuleFor(x => x.PageIndex).GreaterThanOrEqualTo(0);
            RuleFor(x => x.PageSize).GreaterThan(0);

            ApplyConcreteRules();
        }

        protected virtual void ApplyConcreteRules()
        { }
    }
}