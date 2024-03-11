namespace ThreadboxApi.ORM.Entities
{
    public class Tripcode : BaseEntity
    {
        public string Key { get; set; }
        public byte[] Salt { get; set; }
        public byte[] Hash { get; set; }
        public List<Thread> Threads { get; set; }
        public List<Post> Posts { get; set; }
    }
}