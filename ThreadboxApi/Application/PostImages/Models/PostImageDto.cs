using AutoMapper;
using Microsoft.Extensions.Options;
using ThreadboxApi.Application.Common.Constants;
using ThreadboxApi.Application.Common.Mapping.Interfaces;
using ThreadboxApi.ORM.Entities;
using ThreadboxApi.Web;

namespace ThreadboxApi.Application.PostImages.Models
{
    public class PostImageDto : IMapped
    {
        public Guid FileInfoId { get; set; }
        public string Url { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<PostImage, PostImageDto>()
                .ForMember<string>(destination => destination.Url, options => options.MapFrom<PostImageDtoUrlResolver>());
        }
    }

    public class PostImageDtoUrlResolver : IValueResolver<PostImage, PostImageDto, string>
    {
        private readonly IOptionsSnapshot<AppSettings> _appSettings;

        public PostImageDtoUrlResolver(IOptionsSnapshot<AppSettings> appSettings)
        {
            _appSettings = appSettings;
        }

        public string Resolve(
            PostImage source,
            PostImageDto destination,
            string destMember,
            ResolutionContext context)
        {
            return string.Format(WebConstants.FileUrl, _appSettings.Value.BaseUrl, source.FileInfoId);
        }
    }
}