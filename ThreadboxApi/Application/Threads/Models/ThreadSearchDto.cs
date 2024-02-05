using AutoMapper;
using ThreadboxApi.Application.Common.Mapping.Interfaces;
using ThreadboxApi.Application.Posts.Models;
using ThreadboxApi.Web;

namespace ThreadboxApi.Application.Threads.Models
{
    public class ThreadSearchDto : IMapped
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public List<string> ThreadImageUrls { get; set; }
        public List<PostDto> Posts { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ORM.Entities.Thread, ThreadSearchDto>()
                .ForMember(d => d.ThreadImageUrls, o => o.MapFrom<ThreadSearchDtoImageUrlsResolver>());
        }
    }

    public class ThreadSearchDtoImageUrlsResolver : IValueResolver<ORM.Entities.Thread, ThreadSearchDto, List<string>>
    {
        private readonly Services.ApplicationContext _appContext;

        public ThreadSearchDtoImageUrlsResolver(Services.ApplicationContext appContext)
        {
            _appContext = appContext;
        }

        public List<string> Resolve(ORM.Entities.Thread source, ThreadSearchDto destination, List<string> destMember, ResolutionContext context)
        {
            var baseUrl = _appContext.BaseUrl;

            return source.ThreadImages
                .Select(x => string.Format(WebConstants.FileUrl, baseUrl, x.FileInfoId))
                .ToList();
        }
    }
}