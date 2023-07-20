using AutoMapper;
using ThreadboxApi.Application.Common.Helpers;
using ThreadboxApi.Web;

namespace ThreadboxApi.Dtos
{
    public class ListThreadDto : IMapped
	{
		public Guid Id { get; set; }
		public string Title { get; set; }
		public string Text { get; set; }
		public List<string> ThreadImageUrls { get; set; }
		public List<ListPostDto> Posts { get; set; }

		public void Mapping(Profile profile)
		{
			/// Mapping for <see cref="Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry.Entity"/>.
			/// In other cases manual mapping must be used to reduce database load
			profile.CreateMap<Domain.Entities.Thread, ListThreadDto>()
				.ForMember(
					d => d.ThreadImageUrls,
					o => o.MapFrom(s => s.ThreadImages.Select(x => string.Format(ImageRequestConstants.ThreadImageRequestUrl, x.Id)).ToList()));
		}
	}

	public class ThreadDto
	{
		public Guid BoardId { get; set; }
		public string Title { get; set; }
		public string Text { get; set; }
		public List<IFormFile> ThreadImages { get; set; }
	}
}