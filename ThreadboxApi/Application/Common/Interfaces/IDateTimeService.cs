namespace ThreadboxApi.Application.Common.Interfaces
{
    public interface IDateTimeService : IScopedService
    {
        DateTime UtcNow { get; }
    }
}