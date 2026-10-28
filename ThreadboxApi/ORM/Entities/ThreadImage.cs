using ThreadboxApi.ORM.Entities.Interfaces;

namespace ThreadboxApi.ORM.Entities
{
    public class ThreadImage : BaseEntity, IFileDto
    {
        public Guid FileInfoId { get; set; }
        public FileInfo FileInfo { get; set; }
        public Guid ThreadId { get; set; }
        public Thread Thread { get; set; }
    }
}