using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThreadboxApi.Domain.Common;
using ThreadboxApi.Tools;

namespace ThreadboxApi.Domain.Entities
{
    public class PostImage : FileEntity<PostImage>, IMapFromFormFile
    {
        public Guid PostId { get; set; }
        public Post Post { get; set; }
    }
}