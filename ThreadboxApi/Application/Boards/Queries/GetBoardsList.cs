using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ThreadboxApi.Application.Boards.Models;
using ThreadboxApi.Infrastructure.Persistence;

namespace ThreadboxApi.Application.Boards.Queries
{
    public class GetBoardsList : IRequestHandler<GetBoardsList.Query, List<BoardListDto>>
    {
        public class Query : IRequest<List<BoardListDto>>
        { }

        private readonly Infrastructure.Persistence.ApplicationDbContext _dbContext;
        private IMapper _mapper;

        public GetBoardsList(Infrastructure.Persistence.ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<List<BoardListDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var boards = await _dbContext.Boards
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var dtos = _mapper.Map<List<BoardListDto>>(boards);
            return dtos;
        }
    }
}