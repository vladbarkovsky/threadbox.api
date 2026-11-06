using AutoMapper;
using System.ComponentModel.DataAnnotations;
using ThreadboxApi.Application.Common.Mapping;
using ThreadboxApi.ORM.Entities;

namespace ThreadboxApi.Application.PostImages.Models
{
    public class PostImageDto : IMapped
    {
        [Required]
        public Guid FileInfoId { get; init; }
        [Required]
        public string Url { get; init; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<PostImage, PostImageDto>()
                .ForMember(x => x.FileInfoId, x => x.MapFrom(x => x.FileInfoId))
                .ForMember(x => x.Url, x => x.MapFrom<FileUrlResolver<PostImage, PostImageDto>>());
        }
    }
}