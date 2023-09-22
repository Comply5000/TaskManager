namespace TaskManager.Core.Files.Services;

public interface IFileSizeService
{
    Task CheckMaxFilesSize(long fileSize, Guid taskId, CancellationToken cancellationToken);
}