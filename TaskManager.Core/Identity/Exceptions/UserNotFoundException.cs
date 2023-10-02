using TaskManager.Shared.Abstractions.Exceptions;

namespace TaskManager.Core.Identity.Exceptions;

public sealed class UserNotFoundException : TaskManagerException
{
    public UserNotFoundException() : base("User not found") { }
}