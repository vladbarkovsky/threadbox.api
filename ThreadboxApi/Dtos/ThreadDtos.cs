using AutoMapper;
using System.Buffers.Text;
using ThreadboxApi.Configuration;
using ThreadboxApi.Models;
using ThreadboxApi.Tools;

namespace ThreadboxApi.Dtos
{
	public class ListThreadDto : IMapped
	{
		public Guid Id { get; set; }
		public string Title { get; set; } = null!;
		public string Text { get; set; } = null!;
		public List<string> ThreadImageUrls { get; set; } = null!;
		public List<ListPostDto> Posts { get; set; } = null!;

		public void Mapping(Profile profile)
		{
			profile.CreateMap<ThreadModel, ListThreadDto>()
				.ForMember(
					d => d.ThreadImageUrls,
					o => o.MapFrom(s => s.ThreadImages.Select(x => $"{Constants.DevelopmentBaseUrl}/Images/GetThreadImage?imageId={x.Id}")));
		}
	}

	public class ThreadDto
	{
		public string Title { get; set; } = null!;
		public string Text { get; set; } = null!;
		public List<ImageDto> ThreadImages { get; set; } = null!;
	}
}