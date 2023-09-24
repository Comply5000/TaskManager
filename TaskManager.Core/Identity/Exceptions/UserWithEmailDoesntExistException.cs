using TaskManager.Shared.Abstractions.Exceptions;

namespace TaskManager.Core.Identity.Exceptions;

public sealed class UserWithEmailDoesntExistException : TaskManagerException
{
    public UserWithEmailDoesntExistException() : base("User with that email doesn't exist.") { }
}