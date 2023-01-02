using AutoMapper;
using ThreadboxApi.Configuration;
using ThreadboxApi.Models;
using ThreadboxApi.Tools;

namespace ThreadboxApi.Dtos
{
	public class ListPostDto : IMapped
	{
		public Guid Id { get; set; }
		public string Text { get; set; } = null!;
		public Guid ThreadId { get; set; }
		public List<string> PostImageUrls { get; set; } = null!;

		public void Mapping(Profile profile)
		{
			profile.CreateMap<Post, ListPostDto>()
				.ForMember(
					d => d.PostImageUrls,
					o => o.MapFrom(s => s.PostImages.Select(x => $"{Constants.DevelopmentBaseUrl}/Images/GetPostImage?imageId={x.Id}")));
		}
	}

	public class PostDto
	{
		public string Text { get; set; } = null!;
		public List<IFormFile> PostImages { get; set; } = null!;
	}
}