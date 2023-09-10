using Microsoft.AspNetCore.Http;
using TaskManager.Application;
using TaskManager.Core.Files.DTOs;

namespace TaskManager.Core.Files.Services;

public interface IFileStorageService
{
     Task<FileUploadResponseDto> UploadAsync(IFormFile file, CancellationToken cancellationToken);
     Task<MemoryStream> DownloadAsync(byte[] data, CancellationToken cancellationToken);
}