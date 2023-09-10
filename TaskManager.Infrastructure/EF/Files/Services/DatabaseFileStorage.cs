using Microsoft.AspNetCore.Http;
using TaskManager.Application.Files.DTOs;
using TaskManager.Core.Files.DTOs;
using TaskManager.Core.Files.Services;

namespace TaskManager.Infrastructure.EF.Files.Services;

public class DatabaseFileStorage : IFileStorageService
{
    public async Task<FileUploadResponseDto> UploadAsync(IFormFile file, CancellationToken cancellationToken)
    {
        try
        {
            using var memoryStream = new MemoryStream();
            await using var fileStream = file.OpenReadStream();
            var buffer = new byte[1024];
            var totalBytes = file.Length;
            var bytesRead = 0L;
            while (bytesRead < totalBytes)
            {
                var bytes = await fileStream.ReadAsync(buffer, cancellationToken);
                bytesRead += bytes;
                await memoryStream.WriteAsync(buffer.AsMemory(0, bytes), cancellationToken);
                var progressPercent = (double)bytesRead / totalBytes;
            }

            var fileData = memoryStream.ToArray();
            var fileRecord = new FileResponse(fileData, file.Name, file.ContentType);
            await memoryStream.DisposeAsync();
            return new FileUploadResponseDto(true, File: fileRecord);
        }
        catch (Exception e)
        {
            return new FileUploadResponseDto(false, Message: e.Message);
        }
    }
    
    public async Task<MemoryStream> DownloadAsync(byte[] data, CancellationToken cancellationToken)
    {
        var stream = new MemoryStream(data);

        return stream;
    }
}