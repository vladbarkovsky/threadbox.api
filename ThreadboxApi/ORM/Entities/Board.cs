namespace ThreadboxApi.ORM.Entities
{
    public class Board : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<Thread> Threads { get; set; } = new();
    }
}