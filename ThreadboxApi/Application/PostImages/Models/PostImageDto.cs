using AutoMapper;
using ThreadboxApi.Application.Common.Mapping.Interfaces;
using ThreadboxApi.Application.Services;
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
                .ForMember(d => d.Url, o => o.MapFrom<PostImageDtoUrlResolver>());
        }
    }

    public class PostImageDtoUrlResolver : IValueResolver<PostImage, PostImageDto, string>
    {
        private readonly ApplicationContext _appContext;

        public PostImageDtoUrlResolver(ApplicationContext appContext)
        {
            _appContext = appContext;
        }

        public string Resolve(
            PostImage source,
            PostImageDto destination,
            string destMember,
            ResolutionContext context)
        {
            return string.Format(WebConstants.FileUrl, _appContext.BaseUrl, source.FileInfoId);
        }
    }
}