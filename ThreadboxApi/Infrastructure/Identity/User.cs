using Microsoft.AspNetCore.Identity;
using ThreadboxApi.Application.Common.Helpers;
using ThreadboxApi.Dtos;

namespace ThreadboxApi.Infrastructure.Identity
{
    public class User : IdentityUser<Guid>, IMappedFrom<RegistrationFormDto>
    {
        public User()
            : base()
        { }

        public User(string userName)
            : base(userName)
        { }
    }
}