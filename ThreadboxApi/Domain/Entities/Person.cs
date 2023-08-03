using ThreadboxApi.Application.Common.Helpers.Mapping.Interfaces;
using ThreadboxApi.Domain.Common;
using ThreadboxApi.Infrastructure.Identity;

namespace ThreadboxApi.Domain.Entities
{
    public class Person : BaseEntity
    {
        public string UserName { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}