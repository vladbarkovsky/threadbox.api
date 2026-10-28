namespace ThreadboxApi.ORM.Entities.Interfaces
{
    public interface IConsistent
    {
        public uint RowVersion { get; set; }
    }
}
