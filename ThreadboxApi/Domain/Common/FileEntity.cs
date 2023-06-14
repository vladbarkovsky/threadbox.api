namespace ThreadboxApi.Domain.Common
{
    public abstract class FileEntity<TEntity> : BaseEntity<TEntity>
        where TEntity : FileEntity<TEntity>
    {
        public Entities.Owned.File File { get; set; }
    }
}