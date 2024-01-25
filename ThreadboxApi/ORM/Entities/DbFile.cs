namespace ThreadboxApi.ORM.Entities
{
    public class DbFile : BaseEntity
    {
        public string Path { get; set; }
        public byte[] Data { get; set; }
    }
}