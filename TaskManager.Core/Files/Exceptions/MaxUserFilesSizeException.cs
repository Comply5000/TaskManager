using TaskManager.Shared.Abstractions.Exceptions;

namespace TaskManager.Core.Files.Exceptions;

public sealed class MaxUserFilesSizeException : TaskManagerException
{
    public MaxUserFilesSizeException() : base("The maximum file size for user is 100MB") { }
}