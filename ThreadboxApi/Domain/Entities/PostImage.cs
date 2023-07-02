using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThreadboxApi.Application.Common.Helpers;
using ThreadboxApi.Domain.Common;

namespace ThreadboxApi.Domain.Entities
{
    public class PostImage : FileEntity<PostImage>, IMapFromFormFile
    {
        public Guid PostId { get; set; }
        public Post Post { get; set; }
    }
}