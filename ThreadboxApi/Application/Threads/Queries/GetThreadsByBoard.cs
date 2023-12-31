﻿using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ThreadboxApi.Application.Common.Helpers.Pagination;
using ThreadboxApi.Application.Threads.Models;
using ThreadboxApi.Infrastructure.Persistence;

namespace ThreadboxApi.Application.Threads.Queries
{
    public class GetThreadsByBoard : IRequestHandler<GetThreadsByBoard.Query, PaginatedResult<ThreadDto>>
    {
        public class Query : PaginatedQuery, IRequest<PaginatedResult<ThreadDto>>
        {
            public Guid BoardId { get; set; }

            public class Validator : PaginatedQueryValidatorTemplate<Query>
            {
                protected override void OnConfiguring()
                {
                    RuleFor(x => x.BoardId).NotEmpty();
                }
            }
        }

        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetThreadsByBoard(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<ThreadDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var paginatedThreads = await _dbContext.Threads
                .AsNoTracking()
                .AsSplitQuery()
                .Select(thread => new Domain.Entities.Thread
                {
                    Id = thread.Id,
                    Title = thread.Title,
                    Text = thread.Text,
                    BoardId = thread.BoardId,
                    Posts = thread.Posts.Take(3).ToList(),
                    ThreadImages = thread.ThreadImages
                })
                .ToPaginatedResultAsync(request, cancellationToken);

            var paginatedResult = _mapper.Map<PaginatedResult<ThreadDto>>(paginatedThreads);
            return paginatedResult;
        }
    }
}