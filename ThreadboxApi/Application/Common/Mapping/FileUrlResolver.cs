using AutoMapper;
using Microsoft.Extensions.Options;
using ThreadboxApi.Application.Common.Constants;
using ThreadboxApi.ORM.Entities.Interfaces;

namespace ThreadboxApi.Application.Common.Mapping
{
    public class FileUrlResolver<TSource, TDestination> : IValueResolver<TSource, TDestination, string>
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
            return _appSettings.Value.BaseUrl + "/api/files/" + source.FileInfoId;
        }
    }
}
