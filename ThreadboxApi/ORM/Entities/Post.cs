namespace ThreadboxApi.ORM.Entities
{
    public class Post : BaseEntity
    {
        public string Text { get; set; }
        public Guid ThreadId { get; set; }
        public Thread Thread { get; set; }
        public Guid? TripcodeId { get; set; }
        public Tripcode Tripcode { get; set; }
        public List<PostImage> PostImages { get; set; } = new();
    }
}