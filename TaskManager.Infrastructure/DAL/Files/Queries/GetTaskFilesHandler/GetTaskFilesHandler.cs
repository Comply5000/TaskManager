using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManager.Application.Files.Queries.GetTaskFiles;
using TaskManager.Core.Files.DTOs;
using TaskManager.Core.Files.Services;
using TaskManager.Core.Tasks.Exceptions;
using TaskManager.Infrastructure.DAL.EF.Context;

namespace TaskManager.Infrastructure.DAL.Files.Queries.GetTaskFilesHandler;

public sealed class GetTaskFilesHandler : IRequestHandler<GetTaskFiles, GetTaskFilesResponse>
{
    private readonly EFContext _context;
    private readonly IS3StorageService _s3StorageService;
    private readonly IFileZipService _fileZipService;

    public GetTaskFilesHandler(EFContext context, IS3StorageService s3StorageService, IFileZipService fileZipService)
    {
        _context = context;
        _s3StorageService = s3StorageService;
        _fileZipService = fileZipService;
    }
    
    public async Task<GetTaskFilesResponse> Handle(GetTaskFiles request, CancellationToken cancellationToken)
    {
        var task = await _context.Tasks.AsNoTracking()
            .Include(x => x.Files)
            .Where(x => x.Id == request.TaskId)
            .FirstOrDefaultAsync(cancellationToken) ?? throw new TaskNotFoundException();


        var files = new List<FileStreamDto>();
        foreach (var file in task.Files)
        {
            var stream = await _s3StorageService.GetFileAsync(file.S3Key, cancellationToken);
            files.Add(new FileStreamDto(file.Name, stream));
        }
        
        var zip = await _fileZipService.CreateZipWithFilesAsync(files, cancellationToken);

        return new GetTaskFilesResponse(zip, "application/zip", task.Name);
    }
}