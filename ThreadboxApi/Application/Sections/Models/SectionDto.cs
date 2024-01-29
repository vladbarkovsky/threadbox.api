using ThreadboxApi.Application.Boards.Models;
using ThreadboxApi.Application.Common.Mapping.Interfaces;
using ThreadboxApi.ORM.Entities;

namespace ThreadboxApi.Application.Sections.Models
{
    public class SectionDto : IMappedFrom<Section>
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public List<SectionBoardDto> Boards { get; set; }
    }
}