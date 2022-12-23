namespace ThreadboxApi.Models
{
	public class ThreadModel : IEntity
	{
		public Guid Id { get; set; }
		public string Title { get; set; } = null!;
		public string Text { get; set; } = null!;
		public Guid BoardId { get; set; }
		public List<Post> Posts { get; set; } = null!;
		public List<ThreadImage> ThreadImages { get; set; } = null!;
	}
}