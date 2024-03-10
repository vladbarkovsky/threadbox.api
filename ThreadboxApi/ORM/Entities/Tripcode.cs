namespace ThreadboxApi.ORM.Entities
{
    public class Tripcode : BaseEntity
    {
        public string Key { get; set; }
        public byte[] Salt { get; set; }
        public byte[] Hash { get; set; }
        public Thread Thread { get; set; }
        public Post Post { get; set; }
    }
}