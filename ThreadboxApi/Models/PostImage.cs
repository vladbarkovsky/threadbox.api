using AutoMapper;
using ThreadboxApi.Tools;

namespace ThreadboxApi.Models
{
	public class PostImage : IThreadboxFile, IEntity, IMapped
	{
		public Guid Id { get; set; }
		public Guid PostId { get; set; }
		public string Name { get; set; } = null!;
		public string Extension { get; set; } = null!;
		public string ContentType { get; set; } = null!;
		public byte[] Data { get; set; } = null!;
		public Post Post { get; set; } = null!;

		public void Mapping(Profile profile)
		{
			profile.CreateMap<ThreadboxFile, PostImage>();

			profile.CreateMap<IFormFile, PostImage>()
				.IncludeBase<IFormFile, IThreadboxFile>();
		}
	}
}