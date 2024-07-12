using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Application.Files.Queries.GetFile;
using TaskManager.Core.Files.Services;
using TaskManager.Infrastructure.DAL.EF.Context;

namespace TaskManager.Infrastructure.DAL.Files.Queries.GetFileHandler;

public sealed class GetFileHandler : IRequestHandler<GetFile, FileContentResult>
{
    private readonly EFContext _context;
    private readonly IFileStorageService _fileStorageService;
    private readonly IS3StorageService _s3StorageService;

    public GetFileHandler(EFContext context, IFileStorageService fileStorageService, IS3StorageService s3StorageService)
    {
        _context = context;
        _fileStorageService = fileStorageService;
        _s3StorageService = s3StorageService;
    }
    
    public async Task<FileContentResult> Handle(GetFile request, CancellationToken cancellationToken)
    {
        var file = await _context.Files.AsNoTracking()
                       .Where(x => x.Id == request.FileId)
                       .FirstOrDefaultAsync(cancellationToken)
                   ?? throw new FileNotFoundException();

        var stream = await _s3StorageService.GetFileAsync(file.S3Key, cancellationToken);

        await using var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream, cancellationToken);
        var content = memoryStream.ToArray();
        
        // Tworzenie FileContentResult
        var result = new FileContentResult(content, file.ContentType)
        {
            FileDownloadName = file.Name
        };

        return result;
    }
}