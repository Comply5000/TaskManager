using TaskManager.Shared.Abstractions.Exceptions;

namespace TaskManager.Core.Identity.Exceptions;

public sealed class ConfirmAccountException : TaskManagerException
{
    public ConfirmAccountException() : base("An error occurred while confirming the account.") { }
}