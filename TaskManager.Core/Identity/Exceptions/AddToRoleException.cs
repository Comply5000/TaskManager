using TaskManager.Shared.Abstractions.Exceptions;

namespace TaskManager.Core.Identity.Exceptions;

public sealed class AddToRoleException : TaskManagerException
{
    public AddToRoleException() : base("Error while adding role to user") { }
}