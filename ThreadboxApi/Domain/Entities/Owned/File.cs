using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ThreadboxApi.Application.Common.Helpers;

namespace ThreadboxApi.Domain.Entities.Owned
{
    /// <summary>
    /// File with data in byte array
    /// </summary>
    [Owned]
    public class File : IMapped
    {
        /// <summary>
        /// File name with extension ('file.ext')
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// File extension ('.ext')
        /// </summary>
        public string Extension { get; set; }

        /// <summary>
        /// Content-Type HTTP header value
        /// </summary>
        public string ContentType { get; set; }

        public byte[] Data { get; set; }

        public void Mapping(Profile profile)
        {
            // Mapping from multipart/form-data (file sending to server)
            profile.CreateMap<IFormFile, File>()
                .ForMember(d => d.Name, o => o.MapFrom(s => s.FileName))
                .ForMember(d => d.Extension, o => o.MapFrom(s => Path.GetExtension(s.FileName)))
                .ForMember(d => d.Data, o => o.MapFrom<FileDataResolver>());
        }

        /// <summary>
        /// Converts multipart/form-data file to byte array
        /// </summary>
        public class FileDataResolver : IValueResolver<IFormFile, File, byte[]>
        {
            public byte[] Resolve(IFormFile source, File destination, byte[] destMember, ResolutionContext context)
            {
                using var memoryStream = new MemoryStream();
                source.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}