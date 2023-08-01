using Microsoft.AspNetCore.Identity;
using ThreadboxApi.Application.Common.Helpers.Mapping.Interfaces;
using ThreadboxApi.Domain.Entities;

namespace ThreadboxApi.Infrastructure.Identity
{
    public class AppUser : IdentityUser, IMappedFrom<Person>
    {
        public Person Person { get; set; }
    }
}