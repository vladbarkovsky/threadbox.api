using ThreadboxApi.Tools;

namespace ThreadboxApi.Models
{
	public class PostImage : Image, IEntity, IMappedFrom<Image>
	{
		public Guid Id { get; set; }
		public Guid PostId { get; set; }
		public Post Post { get; set; } = null!;
	}
}