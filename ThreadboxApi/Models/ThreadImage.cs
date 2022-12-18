namespace ThreadboxApi.Models
{
    public class ThreadImage : IImage
    {
        public Guid Id { get; set; }
        public string Extension { get; set; } = null!;
        public byte[] Data { get; set; } = null!;
        public Guid ThreadId { get; set; }
        public Thread Thread { get; set; } = null!;
    }
}