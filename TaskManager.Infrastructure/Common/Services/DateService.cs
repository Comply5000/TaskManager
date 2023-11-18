using TaskManager.Core.Shared.Services;

namespace TaskManager.Infrastructure.EF.Shared.Services;

public sealed class DateService : IDateService
{
    public DateTimeOffset CurrentOffsetDate() => DateTimeOffset.UtcNow;
    public DateTime CurrentDate() => DateTime.UtcNow;
}