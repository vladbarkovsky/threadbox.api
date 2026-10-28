using AutoMapper;
using ThreadboxApi.Application.Common.Mapping;
using ThreadboxApi.Application.PostImages.Models;
using ThreadboxApi.ORM.Entities;
using ThreadboxApi.ORM.Entities.Interfaces;

namespace ThreadboxApi.Application.Posts.Models
{
    public class PostDto : IConsistent, IMapped
    {
        public Guid Id { get; init; }
        public uint RowVersion { get; set; }
        public string Text { get; init; }
        public Guid ThreadId { get; init; }
        public string TripcodeKey { get; init; }
        public List<PostImageDto> PostImages { get; init; }
        public DateTimeOffset CreatedAt { get; init; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Post, PostDto>()
                .ForMember(x => x.Id, x => x.MapFrom(x => x.Id))
                .ForMember(x => x.RowVersion, x => x.MapFrom(x => x.RowVersion))
                .ForMember(x => x.Text, x => x.MapFrom(x => x.Text))
                .ForMember(x => x.ThreadId, x => x.MapFrom(x => x.ThreadId))
                .ForMember(x => x.TripcodeKey, x => x.MapFrom((x, _) => x.Tripcode?.Key))
                .ForMember(x => x.PostImages, x => x.MapFrom(x => x.PostImages))
                .ForMember(x => x.CreatedAt, x => x.MapFrom(x => x.CreatedAt));
        }
    }
}
