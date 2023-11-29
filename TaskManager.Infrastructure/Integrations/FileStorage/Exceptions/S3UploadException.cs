using TaskManager.Shared.Abstractions.Exceptions;

namespace TaskManager.Infrastructure.Integrations.FileStorage.Exceptions;

public sealed class S3UploadException : TaskManagerException
{
    public S3UploadException(string errorCode) : base($"Unexpected error while uploading file to S3Storage: {errorCode}") { }
}