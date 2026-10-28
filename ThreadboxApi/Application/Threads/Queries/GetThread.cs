using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ThreadboxApi.Application.Threads.Models;
using ThreadboxApi.ORM.Services;
using ThreadboxApi.Web.ErrorHandling;

namespace ThreadboxApi.Application.Threads.Queries
{
    public class GetThread : IRequestHandler<GetThread.Query, ThreadDto>
    {
        public class Query : IRequest<ThreadDto>
        {
            public Guid Id { get; init; }

            public class Validator : AbstractValidator<Query>
            {
                public Validator()
                {
                    RuleFor(x => x.Id).NotEmpty();
                }
            }
        }

        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetThread(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ThreadDto> Handle(Query request, CancellationToken cancellationToken)
        {
            ORM.Entities.Thread thread = await _dbContext.Threads
                .AsNoTracking()
                .Where(x => x.Id == request.Id)
                .Include(x => x.Tripcode)
                .Include(x => x.Posts)
                .ThenInclude(x => x.Tripcode)
                .Include(x => x.Posts)
                .ThenInclude(x => x.PostImages)
                .Include(x => x.ThreadImages)
                .SingleOrDefaultAsync(cancellationToken);

            if (thread == null)
            {
                throw new HttpResponseException($"Thread with ID \"{request.Id}\" not found.", StatusCodes.Status404NotFound);
            }

            return _mapper.Map<ThreadDto>(thread);
        }
    }
}
