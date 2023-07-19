﻿using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ThreadboxApi.Application.Boards.Models;
using ThreadboxApi.Application.Common.Helpers;
using ThreadboxApi.Infrastructure.Persistence;

namespace ThreadboxApi.Application.Boards.Queries
{
    public class GetBoard : IRequestHandler<GetBoard.Query, BoardDto>
    {
        public class Query : IRequest<BoardDto>
        {
            public Guid BoardId { get; set; }

            public class Validator : AbstractValidator<Query>
            {
                public Validator()
                {
                    RuleFor(x => x.BoardId).NotEmpty();
                }
            }
        }

        private readonly ThreadboxDbContext _dbContext;
        private IMapper _mapper;

        public GetBoard(ThreadboxDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<BoardDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var board = await _dbContext.Boards
                .AsNoTracking()
                .Where(x => x.Id == request.BoardId)
                .FirstOrDefaultAsync(cancellationToken);

            if (board == null)
            {
                throw HttpResponseException.NotFound;
            }

            var dto = _mapper.Map<BoardDto>(board);
            return dto;
        }
    }
}