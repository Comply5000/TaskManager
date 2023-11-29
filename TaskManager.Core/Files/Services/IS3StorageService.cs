using Microsoft.AspNetCore.Http;

namespace TaskManager.Core.Files.Services;

public interface IS3StorageService
{
    Task<string> UploadFileAsync(IFormFile file, CancellationToken cancellationToken);
    Task<string> GetFileUrlAsync(string fileKey, string fileName, CancellationToken cancellationToken);
}