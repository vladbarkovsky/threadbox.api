using ThreadboxApi.Application.Common.Helpers;
using ThreadboxApi.Domain.Common;
using ThreadboxApi.Dtos;

namespace ThreadboxApi.Domain.Entities
{
    public class Board : BaseEntity<Board>, IMappedFrom<BoardDto>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<Thread> Threads { get; set; }
    }
}