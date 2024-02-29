using AutoMapper;
using ThreadboxApi.Application.Common.Mapping.Interfaces;
using ThreadboxApi.Application.Services;
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
                .ForMember(d => d.Url, o => o.MapFrom<ThreadImageDtoUrlResolver>());
        }
    }

    public class ThreadImageDtoUrlResolver : IValueResolver<ThreadImage, ThreadImageDto, string>
    {
        private readonly ApplicationContext _appContext;

        public ThreadImageDtoUrlResolver(ApplicationContext appContext)
        {
            _appContext = appContext;
        }

        public string Resolve(
            ThreadImage source,
            ThreadImageDto destination,
            string destMember,
            ResolutionContext context)
        {
            return string.Format(WebConstants.FileUrl, _appContext.BaseUrl, source.FileInfoId);
        }
    }
}