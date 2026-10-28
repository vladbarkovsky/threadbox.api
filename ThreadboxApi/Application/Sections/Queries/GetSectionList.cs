using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ThreadboxApi.Application.Sections.Models;
using ThreadboxApi.ORM.Services;

namespace ThreadboxApi.Application.Sections.Queries
{
    public class GetSectionList : IRequestHandler<GetSectionList.Query, SectionListDto[]>
    {
        public class Query : IRequest<SectionListDto[]> { }

        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetSectionList(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<SectionListDto[]> Handle(Query request, CancellationToken cancellationToken)
        {
            return await _dbContext.Sections
                .AsNoTracking()
                .ProjectTo<SectionListDto>(_mapper.ConfigurationProvider)
                .ToArrayAsync(cancellationToken);
        }
    }
}
