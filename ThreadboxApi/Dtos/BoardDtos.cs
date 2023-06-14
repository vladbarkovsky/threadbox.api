using FluentValidation;
using ThreadboxApi.Domain.Entities;
using ThreadboxApi.Tools;

namespace ThreadboxApi.Dtos
{
    public class BoardDto : IMappedFrom<Board>
	{
		public Guid? Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }

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
		public string Title { get; set; }
	}
}