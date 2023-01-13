using AutoMapper;
using ThreadboxApi.Dtos;
using ThreadboxApi.Tools;

namespace ThreadboxApi.Models
{
	public class ThreadModel : IEntity, IMapped
	{
		public Guid Id { get; set; }
		public string Title { get; set; } = null!;
		public string Text { get; set; } = null!;
		public Guid BoardId { get; set; }
		public List<Post> Posts { get; set; } = null!;
		public List<ThreadImage> ThreadImages { get; set; } = null!;

		public void Mapping(Profile profile)
		{
			profile.CreateMap<ThreadDto, ThreadModel>();
			//.ForMember(d => d.ThreadImages, o => o.Ignore());
		}
	}
}