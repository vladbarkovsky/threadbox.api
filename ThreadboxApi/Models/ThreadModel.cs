using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThreadboxApi.Dtos;
using ThreadboxApi.Tools;

namespace ThreadboxApi.Models
{
	public class ThreadModel : Entity<ThreadModel>, IMappedFrom<ThreadDto>
	{
		public string Title { get; set; } = null!;
		public string Text { get; set; } = null!;
		public Guid BoardId { get; set; }
		public Board Board { get; set; } = null!;
		public List<Post> Posts { get; set; } = null!;
		public List<ThreadImage> ThreadImages { get; set; } = null!;

		public override void Configure(EntityTypeBuilder<ThreadModel> builder)
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