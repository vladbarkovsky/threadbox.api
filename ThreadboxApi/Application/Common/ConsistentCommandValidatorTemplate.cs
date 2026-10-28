using FluentValidation;
using ThreadboxApi.ORM.Entities.Interfaces;

namespace ThreadboxApi.Application.Common
{
    public abstract class ConsistentCommandValidatorTemplate<TCommand> : AbstractValidator<TCommand>
        where TCommand : class, IConsistentCommand
    {
        public ConsistentCommandValidatorTemplate()
        {
            RuleFor(x => x.RowVersion).NotNull();
            Configure();
        }

        protected virtual void Configure() { }
    }
}
