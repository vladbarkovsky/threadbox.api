using ThreadboxApi.Models;
using ThreadboxApi.Tools;

namespace ThreadboxApi.Dtos
{
	public class ListThreadDto
	{
		public Guid Id { get; set; }
		public string Title { get; set; } = null!;
		public string Text { get; set; } = null!;
		public List<string> ThreadImageUrls { get; set; } = null!;
		public List<ListPostDto> Posts { get; set; } = null!;
	}

	public class ThreadDto
	{
		public Guid BoardId { get; set; }
		public string Title { get; set; } = null!;
		public string Text { get; set; } = null!;
		public List<IFormFile> ThreadImages { get; set; } = null!;
	}
}