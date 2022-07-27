using AutoMapper;
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
            RuleFor(x => x.Route).NotEmpty().MaximumLength(5).Must(x => x == x.ToLower());
            RuleFor(x => x.Name).NotEmpty().MaximumLength(30);
        }
    }

    public class SectionMapping : Profile
    {
        public SectionMapping()
        {
            CreateMap<Section, SectionDto>().ReverseMap();
        }
    }


    public class SectionDto
    {
        public Guid Id { get; set; }
        public string Route { get; set; } = null!;
        public string Name { get; set; } = null!;
    }
}