using ThreadboxApi.Application.Common.Interfaces;

namespace ThreadboxApi.Application.Services
{
    public class DateTimeOffsetService : IDateTimeOffsetService
    {
        public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
    }
}