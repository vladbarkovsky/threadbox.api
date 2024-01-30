namespace ThreadboxApi.ORM.Entities
{
    public class Board : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid SectionId { get; set; }
        public Section Section { get; set; }
        public List<Thread> Threads { get; set; } = new();
    }
}