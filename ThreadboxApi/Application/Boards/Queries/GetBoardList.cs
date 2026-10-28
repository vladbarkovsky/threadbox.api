using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ThreadboxApi.Application.Boards.Models;
using ThreadboxApi.ORM.Services;

namespace ThreadboxApi.Application.Boards.Queries
{
    public class GetBoardList : IRequestHandler<GetBoardList.Query, SectionBoardDto[]>
    {
        public class Query : IRequest<SectionBoardDto[]> { }

        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetBoardList(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<SectionBoardDto[]> Handle(Query request, CancellationToken cancellationToken)
        {
            SectionBoardDto[] boards = await _dbContext.Boards
                .AsNoTracking()
                .ProjectTo<SectionBoardDto>(_mapper.ConfigurationProvider)
                .ToArrayAsync(cancellationToken);

            return boards;
        }
    }
}