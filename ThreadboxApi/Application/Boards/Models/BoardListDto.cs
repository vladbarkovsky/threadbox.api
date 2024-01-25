using ThreadboxApi.Application.Common.Mapping.Interfaces;
using ThreadboxApi.ORM.Entities;

namespace ThreadboxApi.Application.Boards.Models
{
    public class BoardListDto : IMappedFrom<Board>
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
    }
}