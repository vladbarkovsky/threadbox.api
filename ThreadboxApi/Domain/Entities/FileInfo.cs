using ThreadboxApi.Domain.Common;

namespace ThreadboxApi.Domain.Entities
{
    public class FileInfo : BaseEntity
    {
        public string Name { get; set; }
        public string ContentType { get; set; }
        public string Path { get; set; }
        public List<ThreadImage> ThreadImages { get; set; }
        public List<PostImage> PostImages { get; set; }
    }
}