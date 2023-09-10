using TaskManager.Shared.Abstractions.Exceptions;

namespace TaskManager.Shared.Exceptions;

public sealed class IdentityErrorException : TaskManagerException
{
    public Dictionary<string, string> Errors { get; }

    public IdentityErrorException(Dictionary<string, string> errors) : base("Identity Error occured")
    {
        Errors = errors;
    }
}