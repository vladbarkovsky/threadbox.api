using AutoMapper;
using ThreadboxApi.Application.Common.Helpers.Mapping.Interfaces;
using ThreadboxApi.Domain.Entities;
using ThreadboxApi.Web;

namespace ThreadboxApi.Application.Posts.Models
{
    public class PostDto : IMapped
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public Guid ThreadId { get; set; }
        public List<string> PostImageUrls { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Post, PostDto>()
                .ForMember(d => d.PostImageUrls, o => o.MapFrom<PostImageUrlsResolver>());
        }
    }

    public class PostImageUrlsResolver : IValueResolver<Post, PostDto, List<string>>
    {
        private readonly Services.ApplicationContext _appContext;

        public PostImageUrlsResolver(Services.ApplicationContext appContext)
        {
            _appContext = appContext;
        }

        public List<string> Resolve(Post source, PostDto destination, List<string> destMember, ResolutionContext context)
        {
            var baseUrl = _appContext.BaseUrl;

            return source.PostImages
                .Select(x => string.Format(WebConstants.FileUrl, baseUrl, x.FileInfoId))
                .ToList();
        }
    }
}