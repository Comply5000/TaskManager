using TaskManager.Core.Files.DTOs;
using TaskManager.Core.Files.Entities;

namespace TaskManager.Core.Files.Services;

public interface IFileZipService
{
    Task<MemoryStream> CreateZipWithFilesAsync(List<FileStreamDto> files, CancellationToken cancellationToken);
}