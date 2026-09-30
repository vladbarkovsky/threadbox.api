using AutoMapper;
using Microsoft.Extensions.Options;
using ThreadboxApi.Application.Common.Constants;
using ThreadboxApi.Application.Common.Mapping.Interfaces;
using ThreadboxApi.ORM.Entities;
using ThreadboxApi.Web;

namespace ThreadboxApi.Application.ThreadImages.Models
{
    public class ThreadImageDto : IMapped
    {
        public Guid FileInfoId { get; set; }
        public string Url { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ThreadImage, ThreadImageDto>()
                .ForMember(destination => destination.Url, options => options.MapFrom<ThreadImageDtoUrlResolver>());
        }
    }

    public class ThreadImageDtoUrlResolver : IValueResolver<ThreadImage, ThreadImageDto, string>
    {
        private readonly IOptionsSnapshot<AppSettings> _appSettings;

        public ThreadImageDtoUrlResolver(IOptionsSnapshot<AppSettings> appSettings)
        {
            _appSettings = appSettings;
        }

        public string Resolve(
            ThreadImage source,
            ThreadImageDto destination,
            string destMember,
            ResolutionContext context)
        {
            return string.Format(WebConstants.FileUrl, _appSettings.Value.BaseUrl, source.FileInfoId);
        }
    }
}