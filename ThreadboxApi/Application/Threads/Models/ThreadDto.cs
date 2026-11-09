using AutoMapper;
using System.ComponentModel.DataAnnotations;
using ThreadboxApi.Application.Common.Mapping;
using ThreadboxApi.Application.Posts.Models;
using ThreadboxApi.Application.ThreadImages.Models;
using ThreadboxApi.ORM.Entities.Interfaces;

namespace ThreadboxApi.Application.Threads.Models
{
    public class ThreadDto : IConsistent, IMapped
    {
        [Required]
        public Guid Id { get; init; }
        [Required]
        public uint RowVersion { get; set; }
        [Required]
        public string Title { get; init; }
        [Required]
        public string Text { get; init; }
        public string TripcodeKey { get; init; }
        [Required]
        public List<PostDto> Posts { get; set; }
        [Required]
        public List<ThreadImageDto> ThreadImages { get; init; }
        [Required]
        public bool HasMorePosts { get; set; }
        [Required]
        public DateTimeOffset CreatedAt { get; init; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ORM.Entities.Thread, ThreadDto>()
                .ForMember(x => x.Id, x => x.MapFrom(x => x.Id))
                .ForMember(x => x.RowVersion, x => x.MapFrom(x => x.RowVersion))
                .ForMember(x => x.Title, x => x.MapFrom(x => x.Title))
                .ForMember(x => x.Text, x => x.MapFrom(x => x.Text))
                .ForMember(x => x.TripcodeKey, x => x.MapFrom((x, _) => x.Tripcode?.Key))
                .ForMember(x => x.Posts, x => x.MapFrom(x => x.Posts))
                .ForMember(x => x.ThreadImages, x => x.MapFrom(x => x.ThreadImages))
                .ForMember(x => x.HasMorePosts, x => x.Ignore())
                .ForMember(x => x.CreatedAt, x => x.MapFrom(x => x.CreatedAt));
        }
    }
}