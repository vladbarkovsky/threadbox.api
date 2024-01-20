using ThreadboxApi.Domain.Common;

namespace ThreadboxApi.Domain.Entities
{
    public class DbFile : BaseEntity
    {
        public string Path { get; set; }
        public byte[] Data { get; set; }
    }
}