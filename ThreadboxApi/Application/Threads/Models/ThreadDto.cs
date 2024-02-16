using AutoMapper;
using ThreadboxApi.Application.Common.Mapping.Interfaces;
using ThreadboxApi.Application.Posts.Models;
using ThreadboxApi.Application.Services;
using ThreadboxApi.Web;

namespace ThreadboxApi.Application.Threads.Models
{
    public class ThreadDto : IMapped
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public List<string> ThreadImageUrls { get; set; }
        public List<PostDto> Posts { get; set; }
        public bool HasMorePosts { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ORM.Entities.Thread, ThreadDto>()
                .ForMember(d => d.ThreadImageUrls, o => o.MapFrom<ThreadDtoImageUrlsResolver>());
        }
    }

    public class ThreadDtoImageUrlsResolver : IValueResolver<ORM.Entities.Thread, ThreadDto, List<string>>
    {
        private readonly ApplicationContext _appContext;

        public ThreadDtoImageUrlsResolver(ApplicationContext appContext)
        {
            _appContext = appContext;
        }

        public List<string> Resolve(
            ORM.Entities.Thread source,
            ThreadDto destination,
            List<string> destMember,
            ResolutionContext context)
        {
            var baseUrl = _appContext.BaseUrl;

            return source.ThreadImages
                .Select(x => string.Format(WebConstants.FileUrl, baseUrl, x.FileInfoId))
                .ToList();
        }
    }
}