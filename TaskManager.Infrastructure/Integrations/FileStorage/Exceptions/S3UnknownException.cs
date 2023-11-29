using TaskManager.Shared.Abstractions.Exceptions;

namespace TaskManager.Infrastructure.Integrations.FileStorage.Exceptions;

public sealed class S3UnknownException : TaskManagerException
{
    public S3UnknownException() : base("Unknown exception while connecting to S3Service") { }
}