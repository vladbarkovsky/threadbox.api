using FluentValidation;
using ThreadboxApi.Models;
using ThreadboxApi.Tools;

namespace ThreadboxApi.Dtos
{
    public class BoardDto : IMapFrom<Board>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;

        public class BoardDtoValidator : AbstractValidator<BoardDto>
        {
            public BoardDtoValidator()
            {
                RuleFor(x => x.Name)
                    .NotEmpty()
                    .MaximumLength(30);

                RuleFor(x => x.Description).MaximumLength(200);
            }
        }
    }
}