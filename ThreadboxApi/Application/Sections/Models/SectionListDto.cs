using AutoMapper;
using System.ComponentModel.DataAnnotations;
using ThreadboxApi.Application.Boards.Models;
using ThreadboxApi.Application.Common.Mapping;
using ThreadboxApi.ORM.Entities;

namespace ThreadboxApi.Application.Sections.Models
{
    public class SectionListDto : IMapped
    {
        [Required]
        public Guid Id { get; init; }
        [Required]
        public string Title { get; init; }
        [Required]
        public List<SectionBoardDto> Boards { get; init; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Section, SectionListDto>()
                .ForMember(x => x.Id, x => x.MapFrom(x => x.Id))
                .ForMember(x => x.Title, x => x.MapFrom(x => x.Title))
                .ForMember(x => x.Boards, x => x.MapFrom(x => x.Boards));
        }
    }
}
