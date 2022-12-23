namespace ThreadboxApi.Models
{
	public class ThreadImage : Image
	{
		public Guid ThreadId { get; set; }
		public ThreadModel Thread { get; set; } = null!;
	}
}