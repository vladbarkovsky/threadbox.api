using ThreadboxApi.Tools;

namespace ThreadboxApi.Models
{
	public class ThreadImage : Image, IEntity, IMappedFrom<Image>
	{
		public Guid Id { get; set; }
		public Guid ThreadId { get; set; }
		public ThreadModel Thread { get; set; } = null!;
	}
}