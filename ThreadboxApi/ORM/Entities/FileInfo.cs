namespace ThreadboxApi.ORM.Entities
{
    public class FileInfo : BaseEntity
    {
        public string Name { get; set; }
        public string ContentType { get; set; }
        public string Path { get; set; }
        public ThreadImage ThreadImage { get; set; }
        public PostImage PostImage { get; set; }
    }
}