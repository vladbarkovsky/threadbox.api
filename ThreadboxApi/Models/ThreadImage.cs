using AutoMapper;
using ThreadboxApi.Dtos;
using ThreadboxApi.Tools;

namespace ThreadboxApi.Models
{
	public class ThreadImage : Image, IEntity, IMapped
	{
		public Guid Id { get; set; }
		public Guid ThreadId { get; set; }
		public ThreadModel Thread { get; set; } = null!;

		public void Mapping(Profile profile)
		{
			profile.CreateMap<Image, ThreadImage>();

			profile.CreateMap<ImageDto, ThreadImage>()
				.ForMember(d => d.Data, o => o.MapFrom(s => Convert.FromBase64String(s.Base64)));
		}
	}
}