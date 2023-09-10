using TaskManager.Shared.Abstractions.Exceptions;

namespace TaskManager.Core.Identity.Exceptions;

public sealed class AddClaimException : TaskManagerException
{
    public AddClaimException() : base("Error while adding claim to user")
    {
    }
}