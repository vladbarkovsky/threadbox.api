using AutoMapper;
using ThreadboxApi.Application.Common.Mapping.Interfaces;
using ThreadboxApi.Application.PostImages.Models;
using ThreadboxApi.ORM.Entities;

namespace ThreadboxApi.Application.Posts.Models
{
    public class PostDto : IMapped
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public Guid ThreadId { get; set; }
        public string TripcodeKey { get; set; }
        public List<PostImageDto> PostImages { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Post, PostDto>()
                .ForMember(d => d.TripcodeKey, o => o.MapFrom((s, d) => s.Tripcode?.Key));
        }
    }
}