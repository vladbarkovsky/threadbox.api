using AutoMapper;
using ThreadboxApi.Configuration;
using ThreadboxApi.Models;
using ThreadboxApi.Tools;

namespace ThreadboxApi.Dtos
{
	public class ListPostDto : IMapped
	{
		public Guid Id { get; set; }
		public string Text { get; set; }
		public Guid ThreadId { get; set; }
		public List<string> PostImageUrls { get; set; }

		public void Mapping(Profile profile)
		{
			/// Mapping for <see cref="Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry.Entity"/>.
			/// In other cases manual mapping must be used to reduce database load
			profile.CreateMap<Post, ListPostDto>()
				.ForMember(
					d => d.PostImageUrls,
					o => o.MapFrom(s => s.PostImages.Select(x => string.Format(Constants.PostImageRequest, x.Id)).ToList()));
		}
	}

	public class PostDto
	{
		public string Text { get; set; }
		public List<IFormFile> PostImages { get; set; }
	}
}