using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThreadboxApi.Tools;

namespace ThreadboxApi.Models
{
	public class ThreadImage : FileEntity<ThreadImage>, IMapped
	{
		public Guid ThreadId { get; set; }
		public ThreadModel Thread { get; set; } 

		public override void Configure(EntityTypeBuilder<ThreadImage> builder)
		{
			base.Configure(builder);
		}

		public void Mapping(Profile profile)
		{
			profile.CreateMap<IFormFile, ThreadImage>().MapFromFormFile();
		}
	}
}