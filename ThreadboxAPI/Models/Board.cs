using ThreadboxApi.Dtos;
using ThreadboxApi.Tools;

namespace ThreadboxApi.Models
{
	public class Board : IEntity, IMappedFrom<BoardDto>
	{
		public Guid Id { get; set; }
		public string Title { get; set; } = null!;
		public string Description { get; set; } = null!;
		public List<ThreadModel> Threads { get; set; } = null!;
	}
}