using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThreadboxApi.Dtos;
using ThreadboxApi.Tools;

namespace ThreadboxApi.Models
{
	public class Board : Entity<Board>, IMappedFrom<BoardDto>
	{
		public string Title { get; set; }
		public string Description { get; set; }
		public List<ThreadModel> Threads { get; set; }

		public override void Configure(EntityTypeBuilder<Board> builder)
		{
			base.Configure(builder);

			builder
				.HasMany(x => x.Threads)
				.WithOne(x => x.Board)
				.HasForeignKey(x => x.BoardId);
		}
	}
}