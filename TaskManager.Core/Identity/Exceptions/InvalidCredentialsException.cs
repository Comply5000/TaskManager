using TaskManager.Shared.Abstractions.Exceptions;

namespace TaskManager.Core.Identity.Exceptions;

public sealed class InvalidCredentialsException : TaskManagerException
{
    public InvalidCredentialsException() : base("Invalid credentials exception")
    {
    }
}