using ThreadboxApi.Dtos;
using ThreadboxApi.Tools;

namespace ThreadboxApi.Models
{
    public class Board : IEntity, IMapFrom<BoardDto>
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; }
        public List<Thread> Threads { get; set; } = null!;
    }
}