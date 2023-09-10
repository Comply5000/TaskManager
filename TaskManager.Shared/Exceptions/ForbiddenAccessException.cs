using TaskManager.Shared.Abstractions.Exceptions;

namespace TaskManager.Shared.Exceptions;

public sealed class ForbiddenAccessException : TaskManagerException
{
    public ForbiddenAccessException() : base("You don't have access to this resource") { }
}