using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ThreadboxAPI.Models
{
    public class Section : IEntityTypeConfiguration<Section>
    {
        public Guid Id { get; set; }
        public string Route { get; set; } = null!;
        public string Name { get; set; } = null!;

        public void Configure(EntityTypeBuilder<Section> builder)
        {
            builder.HasIndex(x => x.Route).IsUnique();
            builder.HasIndex(x => x.Name).IsUnique();
        }
    }

    public class SectionDto : IMapFrom<Section>
    {
        public Guid Id { get; set; }
        public string Route { get; set; } = null!;
        public string Name { get; set; } = null!;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Section, SectionDto>().ReverseMap();
        }
    }

    public class SectionDtoValidator : AbstractValidator<SectionDto>
    {
        public SectionDtoValidator()
        {
            RuleFor(x => x.Route)
                .NotEmpty()
                .MaximumLength(5)
                .Must(x => x == x.ToLower());

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(30);
        }
    }
}