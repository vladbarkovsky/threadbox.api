namespace ThreadboxApi.Domain.Common
{
    public abstract class BaseEntity<TEntity>
        where TEntity : BaseEntity<TEntity>
    {
        public Guid Id { get; set; }
    }
}