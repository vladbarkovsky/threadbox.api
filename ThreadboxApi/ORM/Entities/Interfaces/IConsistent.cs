namespace ThreadboxApi.ORM.Entities.Interfaces
{
    public interface IConsistent
    {
        uint RowVersion { get; set; }
    }
}
