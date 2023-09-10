using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManager.Application.Files.Queries.DownloadFile;
using TaskManager.Core.Files.Services;
using TaskManager.Infrastructure.EF.Context;

namespace TaskManager.Infrastructure.EF.Files.Queries.DownloadFileHandler;

public sealed class DownloadFileHandler : IRequestHandler<DownloadFile, DownloadFileResponse>
{
    private readonly EFContext _context;
    private readonly IFileStorageService _fileStorageService;

    public DownloadFileHandler(EFContext context, IFileStorageService fileStorageService)
    {
        _context = context;
        _fileStorageService = fileStorageService;
    }
    
    public async Task<DownloadFileResponse> Handle(DownloadFile request, CancellationToken cancellationToken)
    {
        var file = await _context.Files.AsNoTracking()
                       .Where(x => x.Id == request.FileId)
                       .FirstOrDefaultAsync(cancellationToken)
                   ?? throw new FileNotFoundException();

        var downloadFileResult = await _fileStorageService.DownloadAsync(file.Data, cancellationToken);

        return new DownloadFileResponse(file.Name, file.ContentType, downloadFileResult);
    }
}