using AutoMapper;
using ThreadboxApi.Tools;

namespace ThreadboxApi.Models
{
	public class ThreadboxFile : IMapped
	{
		public string Name { get; set; } = null!;
		public byte[] Data { get; set; } = null!;

		public void Mapping(Profile profile)
		{
			profile.CreateMap<ThreadImage, ThreadboxFile>()
				.ForMember(d => d.Name, o => o.MapFrom(s => $"{s.Id}.{s.Extension}"));

			profile.CreateMap<PostImage, ThreadboxFile>()
				.ForMember(d => d.Name, o => o.MapFrom(s => $"{s.Id}.{s.Extension}"));
		}
	}
}