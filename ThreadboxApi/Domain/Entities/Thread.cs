using ThreadboxApi.Domain.Common;

namespace ThreadboxApi.Domain.Entities
{
    public class Thread : BaseEntity
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public Guid BoardId { get; set; }
        public Board Board { get; set; }
        public List<Post> Posts { get; set; }
        public List<ThreadImage> ThreadImages { get; set; }
    }
}