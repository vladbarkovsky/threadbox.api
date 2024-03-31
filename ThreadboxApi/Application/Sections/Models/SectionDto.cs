using ThreadboxApi.Application.Common.Mapping.Interfaces;
using ThreadboxApi.ORM.Entities;
using ThreadboxApi.ORM.Entities.Interfaces;

namespace ThreadboxApi.Application.Sections.Models
{
    public class SectionDto : IConsistent, IMappedFrom<Section>
    {
        public Guid Id { get; set; }
        public byte[] RowVersion { get; set; }
        public string Title { get; set; }
    }
}