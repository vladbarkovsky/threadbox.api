using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ThreadboxApi.Application.Sections.Models;
using ThreadboxApi.ORM.Services;
using ThreadboxApi.Web.ErrorHandling;

namespace ThreadboxApi.Application.Sections.Queries
{
    public class GetSection : IRequestHandler<GetSection.Query, SectionDto>
    {
        public class Query : IRequest<SectionDto>
        {
            public Guid Id { get; set; }

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

        public GetSection(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<SectionDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var dto = await _dbContext.Sections
                .Where(x => x.Id == request.Id)
                .ProjectTo<SectionDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(cancellationToken);

            if (dto == null)
            {
                throw new HttpResponseException($"Section with ID \"{request.Id}\" not found.", StatusCodes.Status404NotFound);
            }

            return dto;
        }
    }
}