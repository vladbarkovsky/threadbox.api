using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThreadboxApi.Dtos;
using ThreadboxApi.Tools;

namespace ThreadboxApi.Models
{
	public class Thread : Entity<Thread>, IMappedFrom<ThreadDto>
	{
		public string Title { get; set; }
		public string Text { get; set; }
		public Guid BoardId { get; set; }
		public Board Board { get; set; }
		public List<Post> Posts { get; set; }
		public List<ThreadImage> ThreadImages { get; set; }

		public override void Configure(EntityTypeBuilder<Thread> builder)
		{
			base.Configure(builder);

			builder
				.HasMany(x => x.Posts)
				.WithOne(x => x.Thread)
				.HasForeignKey(x => x.ThreadId);

			builder
				.HasMany(x => x.ThreadImages)
				.WithOne(x => x.Thread)
				.HasForeignKey(x => x.ThreadId);
		}
	}
}