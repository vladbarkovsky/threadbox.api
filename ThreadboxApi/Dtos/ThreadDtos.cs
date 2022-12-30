using AutoMapper;
using ThreadboxApi.Models;
using ThreadboxApi.Tools;

namespace ThreadboxApi.Dtos
{
	public class ThreadVmDto : IMapped
	{
		public Guid Id { get; set; }
		public string Title { get; set; } = null!;
		public string Text { get; set; } = null!;
		public List<string> ThreadImages { get; set; } = null!;

		public void Mapping(Profile profile)
		{
			profile.CreateMap<ThreadModel, ThreadVmDto>()
				.ForMember(d => d.ThreadImages, o => o.MapFrom(s => s.ThreadImages.Select(x => $"{x.Id}.{x.Extension}")));
		}
	}

	public class ThreadDto
	{
		public Guid Id { get; set; }
		public string Title { get; set; } = null!;
		public string Text { get; set; } = null!;
		public List<IFormFile> ThreadImages { get; set; } = null!;
	}
}