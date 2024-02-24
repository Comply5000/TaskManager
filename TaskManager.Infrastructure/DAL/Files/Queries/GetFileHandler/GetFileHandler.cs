using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManager.Application.Files.Queries.GetFile;
using TaskManager.Core.Files.Services;
using TaskManager.Infrastructure.DAL.EF.Context;

namespace TaskManager.Infrastructure.DAL.Files.Queries.GetFileHandler;

public sealed class GetFileHandler : IRequestHandler<GetFile, GetFileResponse>
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
    
    public async Task<GetFileResponse> Handle(GetFile request, CancellationToken cancellationToken)
    {
        var file = await _context.Files.AsNoTracking()
                       .Where(x => x.Id == request.FileId)
                       .FirstOrDefaultAsync(cancellationToken)
                   ?? throw new FileNotFoundException();

        var fileUrl = await _s3StorageService.GetFileUrlAsync(file.S3Key, file.Name, cancellationToken);

        return new GetFileResponse(fileUrl);
    }
}