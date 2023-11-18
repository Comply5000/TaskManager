namespace TaskManager.Core.Shared.Services;

public interface IDateService
{
    DateTimeOffset CurrentOffsetDate();
    DateTime CurrentDate();
}