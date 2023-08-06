using ThreadboxApi.Domain.Common;

namespace ThreadboxApi.Domain.Entities
{
    public class DbFile : BaseEntity
    {
        public string Path { get; set; }

        // This property is lazy loaded. See https://learn.microsoft.com/en-us/ef/ef6/querying/related-data#lazy-loading
        // TODO: Test lazy loading
        public virtual byte[] Data { get; set; }
    }
}