using FluentValidation;
using ThreadboxApi.Models;
using ThreadboxApi.Tools;

namespace ThreadboxApi.Dtos
{
	public class BoardDto : IMappedFrom<Board>
	{
		public Guid Id { get; set; }
		public string Title { get; set; } = null!;
		public string Description { get; set; } = null!;

		public class BoardDtoValidator : AbstractValidator<BoardDto>
		{
			public BoardDtoValidator()
			{
				RuleFor(x => x.Title).NotEmpty();
			}
		}
	}

	public class ListBoardDto : IMappedFrom<Board>
	{
		public Guid Id { get; set; }
		public string Title { get; set; } = null!;
	}
}