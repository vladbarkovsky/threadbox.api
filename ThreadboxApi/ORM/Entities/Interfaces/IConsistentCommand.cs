namespace ThreadboxApi.ORM.Entities.Interfaces
{
    public interface IConsistentCommand
    {
        uint? RowVersion { get; init; }
    }
}
