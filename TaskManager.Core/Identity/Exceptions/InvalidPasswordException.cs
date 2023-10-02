using TaskManager.Shared.Abstractions.Exceptions;

namespace TaskManager.Core.Identity.Exceptions;

public sealed class InvalidPasswordException : TaskManagerException
{
    public InvalidPasswordException() : base("Invalid password") { }
}