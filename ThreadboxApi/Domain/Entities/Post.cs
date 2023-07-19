using ThreadboxApi.Domain.Common;

namespace ThreadboxApi.Domain.Entities
{
    public class Post : BaseEntity
    {
        public string Text { get; set; }
        public Guid ThreadId { get; set; }
        public Thread Thread { get; set; }
        public List<PostImage> PostImages { get; set; }
    }
}