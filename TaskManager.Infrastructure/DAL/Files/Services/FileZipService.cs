using System.IO.Compression;
using TaskManager.Core.Files.DTOs;
using TaskManager.Core.Files.Entities;
using TaskManager.Core.Files.Services;

namespace TaskManager.Infrastructure.DAL.Files.Services;

public sealed class FileZipService : IFileZipService
{
    private readonly IS3StorageService _s3StorageService;

    public FileZipService(IS3StorageService s3StorageService)
    {
        _s3StorageService = s3StorageService;
    }
    
    public async Task<MemoryStream> CreateZipWithFilesAsync(List<FileStreamDto> files, CancellationToken cancellationToken)
    {
        var outputMemStream = new MemoryStream();

        using (var zipArchive = new ZipArchive(outputMemStream, ZipArchiveMode.Create, true))
        {
            foreach (var file in files)
            {
                if (file.Stream.Length > 0)
                {
                    var newEntry = zipArchive.CreateEntry(file.FileName);
                    await using var entryStream = newEntry.Open();
                    using var fileStream = new MemoryStream();
                    await file.Stream.CopyToAsync(fileStream, cancellationToken);
                    fileStream.Position = 0;
                    await fileStream.CopyToAsync(entryStream, cancellationToken);
                }
            }
        }

        outputMemStream.Position = 0;
        return outputMemStream;
    }
}