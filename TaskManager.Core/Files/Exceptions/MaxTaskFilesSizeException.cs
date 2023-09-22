using TaskManager.Shared.Abstractions.Exceptions;

namespace TaskManager.Core.Files.Exceptions;

public sealed class MaxTaskFilesSizeException : TaskManagerException
{
    public MaxTaskFilesSizeException() : base("The maximum file size for single task is 10MB") { }
}