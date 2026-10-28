namespace ThreadboxApi.ORM.Entities
{
    public class DbFile
    {
        public Guid Id { get; set; }
        public string Path { get; set; }
        public byte[] Data { get; set; }
    }
}