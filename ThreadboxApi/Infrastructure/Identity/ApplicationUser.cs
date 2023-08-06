using Microsoft.AspNetCore.Identity;
using ThreadboxApi.Application.Common.Helpers.Mapping.Interfaces;
using ThreadboxApi.Domain.Entities;

namespace ThreadboxApi.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser, IMappedFrom<Person>
    {
        public Person Person { get; set; }
    }
}