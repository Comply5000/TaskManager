using TaskManager.Shared.Abstractions.Exceptions;

namespace TaskManager.Core.Identity.Exceptions;

public sealed class ResetPasswordException : TaskManagerException
{
    public ResetPasswordException() : base("An error occurred while resetting your password.") { }
}