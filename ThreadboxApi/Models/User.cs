using AutoMapper;
using Microsoft.AspNetCore.Identity;
using ThreadboxApi.Dtos;
using ThreadboxApi.Tools;

namespace ThreadboxApi.Models
{
	public class User : IdentityUser<Guid>, IMappedFrom<RegistrationFormDto>
	{
		public User()
			: base()
		{ }

		public User(string userName)
			: base(userName)
		{ }

		public bool IsLocked { get; set; } = true;
	}
}