using AutoMapper;
using System.ComponentModel.DataAnnotations;
using ThreadboxApi.Application.Common.Mapping;
using ThreadboxApi.ORM.Entities;
using ThreadboxApi.ORM.Entities.Interfaces;

namespace ThreadboxApi.Application.Sections.Models
{
    public class SectionDto : IConsistent, IMapped
    {
        [Required]
        public Guid Id { get; init; }
        [Required]
        public uint RowVersion { get; set; }
        [Required]
        public string Title { get; init; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Section, SectionDto>()
                .ForMember(x => x.Id, x => x.MapFrom(x => x.Id))
                .ForMember(x => x.RowVersion, x => x.MapFrom(x => x.RowVersion))
                .ForMember(x => x.Title, x => x.MapFrom(x => x.Title));
        }
    }
}
