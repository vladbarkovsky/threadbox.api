namespace ThreadboxApi.Application.Common.Interfaces
{
    public interface IDateTimeService : IScopedService
    {
        DateTimeOffset UtcNow { get; }
    }
}