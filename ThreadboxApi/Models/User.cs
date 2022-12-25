using Microsoft.AspNetCore.Identity;

namespace ThreadboxApi.Models
{
	public class User : IdentityUser<Guid>
	{
		public bool IsLocked { get; set; }

		public User()
			: base()
		{ }

		public User(string userName)
			: base(userName)
		{ }
	}
}