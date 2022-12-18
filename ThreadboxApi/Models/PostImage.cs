namespace ThreadboxApi.Models
{
    public class PostImage : IImage
    {
        public Guid Id { get; set; }
        public string Extension { get; set; } = null!;
        public byte[] Data { get; set; } = null!;
        public Guid PostId { get; set; }
        public Post Post { get; set; } = null!;
    }
}