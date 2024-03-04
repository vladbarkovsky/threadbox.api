using ThreadboxApi.Application.Common.Mapping.Interfaces;
using ThreadboxApi.Application.Posts.Models;
using ThreadboxApi.Application.ThreadImages.Models;

namespace ThreadboxApi.Application.Threads.Models
{
    public class ThreadDto : IMappedFrom<ORM.Entities.Thread>
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public List<ThreadImageDto> ThreadImages { get; set; }
        public List<PostDto> Posts { get; set; }
        public bool HasMorePosts { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}