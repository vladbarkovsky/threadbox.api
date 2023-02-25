using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ThreadboxApi.Models
{
	public class Post : Entity<Post>
	{
		public string Text { get; set; } = null!;
		public Guid ThreadId { get; set; }
		public ThreadModel Thread { get; set; } = null!;
		public List<PostImage> PostImages { get; set; } = null!;

		public override void Configure(EntityTypeBuilder<Post> builder)
		{
			base.Configure(builder);

			builder
				.HasMany(x => x.PostImages)
				.WithOne(x => x.Post)
				.HasForeignKey(x => x.PostId);
		}
	}
}