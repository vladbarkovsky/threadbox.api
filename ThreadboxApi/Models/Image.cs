namespace ThreadboxApi.Models
{
	public class Image : IEntity
	{
		public Guid Id { get; set; }
		public string Extension { get; set; } = null!;
		public byte[] Data { get; set; } = null!;
	}
}