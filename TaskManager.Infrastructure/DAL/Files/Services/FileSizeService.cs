using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Files.Exceptions;
using TaskManager.Core.Files.Services;
using TaskManager.Core.Identity.Services;
using TaskManager.Infrastructure.DAL.EF.Context;
using TaskManager.Shared;

namespace TaskManager.Infrastructure.EF.Files.Services;

public sealed class FileSizeService : IFileSizeService
{
    private readonly EFContext _context;
    private readonly ICurrentUserService _currentUserService;

    public FileSizeService(EFContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }
    
    public async Task CheckMaxFilesSize(long fileSize, Guid taskId, CancellationToken cancellationToken)
    {
        var taskFilesSize = await _context.Files
            .Where(x => x.TaskId == taskId)
            .SumAsync(x => x.TotalBytes, cancellationToken);
        
        if (taskFilesSize + fileSize > Globals.MaxFileSize)
            throw new MaxTaskFilesSizeException();

        var userFilesSize = await _context.Files
            .Where(x => x.CreatedById == _currentUserService.UserId)
            .SumAsync(x => x.TotalBytes, cancellationToken);
        
        if (userFilesSize + fileSize > Globals.MaxFileSizeForUser)
            throw new MaxUserFilesSizeException();
    }
}