using TaskManager.Shared.Abstractions.Exceptions;

namespace TaskManager.Infrastructure.Integrations.FileStorage.Exceptions;

public sealed class S3GetUrlException : TaskManagerException
{
    public S3GetUrlException(string errorCode) : base($"Unexpected error while getting URL file from S3Storage: {errorCode}") { }
}