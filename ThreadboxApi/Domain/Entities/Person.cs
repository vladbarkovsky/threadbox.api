﻿using ThreadboxApi.Domain.Common;
using ThreadboxApi.Infrastructure.Identity;

namespace ThreadboxApi.Domain.Entities
{
    public class Person : BaseEntity
    {
        public string UserName { get; set; }
        public Guid AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}