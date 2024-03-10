using AutoMapper;
using ThreadboxApi.Application.Common.Mapping.Interfaces;
using ThreadboxApi.Application.Posts.Models;
using ThreadboxApi.Application.ThreadImages.Models;

namespace ThreadboxApi.Application.Threads.Models
{
    public class ThreadDto : IMapped
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string TripcodeKey { get; set; }
        public List<PostDto> Posts { get; set; }
        public List<ThreadImageDto> ThreadImages { get; set; }
        public bool HasMorePosts { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ORM.Entities.Thread, ThreadDto>()
                .ForMember(d => d.TripcodeKey, o => o.MapFrom((s, d) => s.Tripcode?.Key));
        }
    }
}