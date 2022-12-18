namespace ThreadboxApi.Models
{
    public interface IImage : IEntity
    {
        public string Extension { get; set; }
        public byte[] Data { get; set; }
    }
}