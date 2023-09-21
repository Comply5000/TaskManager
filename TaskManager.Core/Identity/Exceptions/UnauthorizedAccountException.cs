using TaskManager.Shared.Abstractions.Exceptions;

namespace TaskManager.Core.Identity.Exceptions;

public sealed class UnauthorizedAccountException : TaskManagerException
{
    public UnauthorizedAccountException() : base("Unable to log in to unauthorized account.") { }
}