namespace ThreadboxApi.ORM.Entities
{
    public class Section : BaseEntity
    {
        public string Title { get; set; }
        public List<Board> Boards { get; set; }
    }
}