using TaskManager.Shared.Abstractions.Exceptions;

namespace TaskManager.Core.Files.Exceptions;

public sealed class FileNotFoundException : TaskManagerException
{
    public FileNotFoundException() : base("File not found")
    {
    }
}