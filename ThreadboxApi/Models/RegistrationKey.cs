using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ThreadboxApi.Models
{
	public class RegistrationKey : Entity<RegistrationKey>
	{
		public DateTimeOffset CreatedAt { get; set; }

		public override void Configure(EntityTypeBuilder<RegistrationKey> builder)
		{
			base.Configure(builder);
		}
	}
}