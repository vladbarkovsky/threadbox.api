using FluentValidation;
using ThreadboxApi.Application.Common.Helpers;
using ThreadboxApi.Domain.Entities;

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