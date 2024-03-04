using ThreadboxApi.Application.Common.Mapping.Interfaces;
using ThreadboxApi.Application.PostImages.Models;
using ThreadboxApi.ORM.Entities;

namespace ThreadboxApi.Application.Posts.Models
{
    public class PostDto : IMappedFrom<Post>
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public Guid ThreadId { get; set; }
        public List<PostImageDto> PostImages { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}