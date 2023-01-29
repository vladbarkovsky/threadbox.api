using AutoMapper;
using ThreadboxApi.Tools;

namespace ThreadboxApi.Models
{
	public class ThreadImage : IEntity, IThreadboxFile, IMapped
	{
		public Guid Id { get; set; }
		public string Name { get; set; } = null!;
		public string Extension { get; set; } = null!;
		public string ContentType { get; set; } = null!;
		public byte[] Data { get; set; } = null!;
		public Guid ThreadId { get; set; }
		public ThreadModel Thread { get; set; } = null!;

		public void Mapping(Profile profile)
		{
			profile.CreateMap<ThreadboxFile, ThreadImage>();

			profile.CreateMap<IFormFile, ThreadImage>()
				.IncludeBase<IFormFile, IThreadboxFile>();
		}
	}
}