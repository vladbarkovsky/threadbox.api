using AutoMapper;
using Microsoft.Extensions.Options;
using ThreadboxApi.Configuration;
using ThreadboxApi.ORM.Entities.Interfaces;

namespace ThreadboxApi.Mapping
{
    public sealed class FileUrlResolver<TSource, TDestination> : IValueResolver<TSource, TDestination, string>
        where TSource : IFileDto
    {
        private readonly IOptionsSnapshot<AppSettings> _appSettings;

        public FileUrlResolver(IOptionsSnapshot<AppSettings> appSettings)
        {
            _appSettings = appSettings;
        }

        public string Resolve(
            TSource source,
            TDestination destination,
            string destinationMember,
            ResolutionContext context)
        {
            return _appSettings.Value.BaseUrl + "/api/files?fileInfoId=" + source.FileInfoId;
        }
    }
}
