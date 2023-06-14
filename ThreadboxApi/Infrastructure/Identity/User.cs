using Microsoft.AspNetCore.Identity;
using ThreadboxApi.Dtos;
using ThreadboxApi.Tools;

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