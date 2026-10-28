using AutoMapper;
using ThreadboxApi.Application.Common.Mapping;
using ThreadboxApi.Application.Posts.Models;
using ThreadboxApi.Application.ThreadImages.Models;

namespace ThreadboxApi.Application.Threads.Models
{
    public class ThreadDto : IMapped
    {
        public Guid Id { get; init; }
        public string Title { get; init; }
        public string Text { get; init; }
        public string TripcodeKey { get; init; }
        public List<PostDto> Posts { get; set; }
        public List<ThreadImageDto> ThreadImages { get; init; }
        public bool HasMorePosts { get; set; }
        public DateTimeOffset CreatedAt { get; init; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ORM.Entities.Thread, ThreadDto>()
                .ForMember(x => x.Id, x => x.MapFrom(x => x.Id))
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