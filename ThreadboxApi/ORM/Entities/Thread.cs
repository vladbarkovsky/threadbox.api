namespace ThreadboxApi.ORM.Entities
{
    public class Thread : BaseEntity
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public Guid BoardId { get; set; }
        public Board Board { get; set; }
        public Guid? TripcodeId { get; set; }
        public Tripcode Tripcode { get; set; }
        public List<Post> Posts { get; set; } = new();
        public List<ThreadImage> ThreadImages { get; set; } = new();
    }
}