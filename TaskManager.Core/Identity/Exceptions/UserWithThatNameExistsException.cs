using TaskManager.Shared.Abstractions.Exceptions;

namespace TaskManager.Core.Identity.Exceptions;

public sealed class UserWithThatNameExistsException : TaskManagerException
{
    public UserWithThatNameExistsException() : base("User with that name exists")
    {
    }
}