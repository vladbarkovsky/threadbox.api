using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ThreadboxApi.Application.Sections.Models;
using ThreadboxApi.ORM.Services;

namespace ThreadboxApi.Application.Sections.Queries
{
    public class GetSections : IRequestHandler<GetSections.Query, List<SectionListDto>>
    {
        public class Query : IRequest<List<SectionListDto>>
        { }

        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetSections(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<List<SectionListDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var dtos = await _dbContext.Sections
                .AsNoTracking()
                .ProjectTo<SectionListDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return dtos;
        }
    }
}