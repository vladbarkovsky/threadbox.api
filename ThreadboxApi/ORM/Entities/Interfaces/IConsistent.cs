namespace ThreadboxApi.ORM.Entities.Interfaces
{
    public interface IConsistent
    {
        public byte[] RowVersion { get; set; }
    }
}
