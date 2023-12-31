﻿using ThreadboxApi.Application.Common.Helpers.Mapping.Interfaces;
using ThreadboxApi.Domain.Entities;

namespace ThreadboxApi.Application.Boards.Models
{
    public class BoardListDto : IMappedFrom<Board>
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
    }
}