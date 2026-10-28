using AutoMapper;
using ThreadboxApi.Application.Boards.Models;
using ThreadboxApi.Application.Common.Mapping;
using ThreadboxApi.ORM.Entities;

namespace ThreadboxApi.Application.Sections.Models
{
    public class SectionListDto : IMapped
    {
        public Guid Id { get; init; }
        public string Title { get; init; }
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
