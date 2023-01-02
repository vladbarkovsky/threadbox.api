using FluentValidation;
using ThreadboxApi.Models;
using ThreadboxApi.Tools;

namespace ThreadboxApi.Dtos
{
	public class BoardDto
	{
		public Guid? Id { get; set; }
		public string Title { get; set; } = null!;
		public string Description { get; set; } = null!;

		public class CreateBoardDtoValidator : AbstractValidator<BoardDto>
		{
			public CreateBoardDtoValidator()
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

	public class ComponentBoardDto : IMappedFrom<Board>
	{
		public string Title { get; set; } = null!;
		public string Description { get; set; } = null!;
	}
}