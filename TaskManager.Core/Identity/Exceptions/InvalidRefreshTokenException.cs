using TaskManager.Shared.Abstractions.Exceptions;

namespace TaskManager.Core.Identity.Exceptions;

public sealed class InvalidRefreshTokenException : TaskManagerException
{
    public InvalidRefreshTokenException() : base("Invalid refresh token") { }
}