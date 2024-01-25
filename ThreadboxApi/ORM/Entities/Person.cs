namespace ThreadboxApi.ORM.Entities
{
    public class Person : BaseEntity
    {
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}