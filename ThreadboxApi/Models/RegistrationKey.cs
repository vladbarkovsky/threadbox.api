namespace ThreadboxApi.Models
{
	public class RegistrationKey : IEntity
	{
		public Guid Id { get; set; }
		public Guid Value { get; set; }
		public DateTimeOffset CreatedAt { get; set; }
	}
}