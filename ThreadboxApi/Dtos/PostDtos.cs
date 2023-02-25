using AutoMapper;
using ThreadboxApi.Models;
using ThreadboxApi.Tools;

namespace ThreadboxApi.Dtos
{
	public class ListPostDto
	{
		public Guid Id { get; set; }
		public string Text { get; set; } = null!;
		public Guid ThreadId { get; set; }
		public List<string> PostImageUrls { get; set; } = null!;
	}

	public class PostDto
	{
		public string Text { get; set; } = null!;
		public List<IFormFile> PostImages { get; set; } = null!;
	}
}