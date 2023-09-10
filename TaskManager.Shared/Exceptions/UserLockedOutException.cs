using TaskManager.Shared.Abstractions.Exceptions;

namespace TaskManager.Shared.Exceptions;

public sealed class UserLockedOutException : TaskManagerException
{
    public Guid UserId { get; }
    public DateTimeOffset? LockoutEnd { get; }
    public string ReasonWhy { get; }

    public UserLockedOutException(Guid userId, DateTimeOffset? lockoutEnd, string reasonWhy) : base(
        "Your account is locked")
    {
        UserId = userId;
        LockoutEnd = lockoutEnd;
        ReasonWhy = reasonWhy;
    }
}