using TaskManager.Shared.Abstractions.Exceptions;

namespace TaskManager.Core.Files.Exceptions;

public sealed class InvalidFileSizeException : TaskManagerException
{
    public InvalidFileSizeException() : base("File size is to big") { }
}