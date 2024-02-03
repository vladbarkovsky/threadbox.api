using AutoMapper;
using ThreadboxApi.Application.Common.Mapping.Interfaces;
using ThreadboxApi.Application.Posts.Models;
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
                .ForMember(d => d.ThreadImageUrls, o => o.MapFrom<ThreadImageUrlsResolver>())
                .ForMember(d => d.Posts, o => o.MapFrom<PostsResolver>())
                .ForMember(d => d.HasMorePosts, o => o.MapFrom(s => s.Posts.Count > 3));
        }
    }

    public class ThreadImageUrlsResolver : IValueResolver<ORM.Entities.Thread, ThreadDto, List<string>>
    {
        private readonly Services.ApplicationContext _appContext;

        public ThreadImageUrlsResolver(Services.ApplicationContext appContext)
        {
            _appContext = appContext;
        }

        public List<string> Resolve(ORM.Entities.Thread source, ThreadDto destination, List<string> destMember, ResolutionContext context)
        {
            var baseUrl = _appContext.BaseUrl;

            return source.ThreadImages
                .Select(x => string.Format(WebConstants.FileUrl, baseUrl, x.FileInfoId))
                .ToList();
        }
    }

    public class PostsResolver : IValueResolver<ORM.Entities.Thread, ThreadDto, List<PostDto>>
    {
        private readonly IMapper _mapper;

        public PostsResolver(IMapper mapper)
        {
            _mapper = mapper;
        }

        public List<PostDto> Resolve(ORM.Entities.Thread source, ThreadDto destination, List<PostDto> destMember, ResolutionContext context)
        {
            /// List of post entities contains maximum of 4 posts
            /// (see <see cref="Queries.GetThreadsByBoard.Handle(Queries.GetThreadsByBoard.Query, CancellationToken)"/>
            /// to understand, why). We need a maximum of 3.
            return _mapper.Map<List<PostDto>>(source.Posts.Take(3));
        }
    }
}