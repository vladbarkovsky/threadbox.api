using AutoMapper;
using ThreadboxApi.Application.Common.Mapping;
using ThreadboxApi.ORM.Entities;

namespace ThreadboxApi.Application.ThreadImages.Models
{
    public class ThreadImageDto : IMapped
    {
        public Guid FileInfoId { get; init; }
        public string Url { get; init; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ThreadImage, ThreadImageDto>()
                .ForMember(x => x.FileInfoId, x => x.MapFrom(x => x.FileInfoId))
                .ForMember(x => x.Url, x => x.MapFrom<FileUrlResolver<ThreadImage, ThreadImageDto>>());
        }
    }
}