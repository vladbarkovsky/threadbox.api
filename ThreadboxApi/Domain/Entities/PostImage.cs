using ThreadboxApi.Domain.Common;

namespace ThreadboxApi.Domain.Entities
{
    public class PostImage : BaseEntity
    {
        public Guid FileInfoId { get; set; }
        public FileInfo FileInfo { get; set; }
        public Guid PostId { get; set; }
        public Post Post { get; set; }
    }
}