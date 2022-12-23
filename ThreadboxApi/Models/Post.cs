namespace ThreadboxApi.Models
{
    public class Post : IEntity
    {
        public Guid Id { get; set; }
        public string Text { get; set; } = null!;
        public Guid ThreadId { get; set; }
        public ThreadModel Thread { get; set; } = null!;
        public List<PostImage> PostImages { get; set; } = null!;
    }
}