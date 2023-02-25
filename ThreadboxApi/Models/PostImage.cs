using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThreadboxApi.Tools;

namespace ThreadboxApi.Models
{
	public class PostImage : FileEntity<PostImage>, IMapped
	{
		public Guid PostId { get; set; }
		public Post Post { get; set; } = null!;

		public override void Configure(EntityTypeBuilder<PostImage> builder)
		{
			base.Configure(builder);
		}

		public void Mapping(Profile profile)
		{
			profile.CreateMap<IFormFile, PostImage>().MapFromFormFile();
		}
	}
}