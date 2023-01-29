using AutoMapper;
using ThreadboxApi.Tools;

namespace ThreadboxApi.Models
{
	public interface IThreadboxFile
	{
		public string Name { get; set; }
		public string Extension { get; set; }
		public string ContentType { get; set; }
		public byte[] Data { get; set; }
	}

	public class ThreadboxFile : IThreadboxFile, IMapped
	{
		public string Name { get; set; } = null!;
		public string Extension { get; set; } = null!;
		public string ContentType { get; set; } = null!;
		public byte[] Data { get; set; } = null!;

		public void Mapping(Profile profile)
		{
			profile.CreateMap<IFormFile, IThreadboxFile>()
				.AsProxy()
				.ForMember(d => d.Name, o => o.MapFrom(s => s.FileName))
				.ForMember(d => d.Extension, o => o.MapFrom(s => Path.GetExtension(s.FileName)))
				.ForMember(d => d.Data, o => o.MapFrom<FileDataResolver>());

			profile.CreateMap<ThreadImage, ThreadboxFile>();
			profile.CreateMap<PostImage, ThreadboxFile>();
		}

		public class FileDataResolver : IValueResolver<IFormFile, IThreadboxFile, byte[]>
		{
			public byte[] Resolve(IFormFile source, IThreadboxFile destination, byte[] destMember, ResolutionContext context)
			{
				using var memoryStream = new MemoryStream();
				source.CopyTo(memoryStream);
				return memoryStream.ToArray();
			}
		}
	}
}