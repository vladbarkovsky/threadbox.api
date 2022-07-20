using FluentValidation;

namespace ThreadboxAPI.Models
{
    public class Section
    {
        public Guid Id { get; set; }

        public string Route { get; set; } = null!;

        public string Name { get; set; } = null!;
    }

    public class SectionValidator : AbstractValidator<Section>
    {
        public SectionValidator()
        {
            RuleFor(x => x.Route).NotEmpty().MaximumLength(5);
            RuleFor(x => x.Name).NotEmpty().MaximumLength(30);
        }
    }
}