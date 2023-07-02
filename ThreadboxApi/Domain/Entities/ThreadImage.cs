using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThreadboxApi.Application.Common.Helpers;
using ThreadboxApi.Domain.Common;

namespace ThreadboxApi.Domain.Entities
{
    public class ThreadImage : FileEntity<ThreadImage>, IMapped
    {
        public Guid ThreadId { get; set; }
        public Thread Thread { get; set; }

        public override void Configure(EntityTypeBuilder<ThreadImage> builder)
        {
            base.Configure(builder);
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<IFormFile, ThreadImage>().MapFromFormFile();
        }
    }
}