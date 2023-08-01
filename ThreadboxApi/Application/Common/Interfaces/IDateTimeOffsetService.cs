namespace ThreadboxApi.Application.Common.Interfaces
{
    public interface IDateTimeOffsetService : IScopedService
    {
        DateTimeOffset UtcNow { get; }
    }
}