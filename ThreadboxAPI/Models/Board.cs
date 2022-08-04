using AutoMapper;
using FluentValidation;

namespace ThreadboxAPI.Models
{
    public class Board
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public List<Section> Sections { get; set; } = null!;
    }

    public class BoardDto : IMapFrom<Board>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Board, BoardDto>().ReverseMap();
        }
    }

    public class BoardDtoValidator : AbstractValidator<BoardDto>
    {
        public BoardDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(30);
            RuleFor(x => x.Description)
                .MaximumLength(200);
        }
    }
}