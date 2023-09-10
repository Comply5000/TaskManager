using MediatR;
using TaskManager.Core.Files.Entities;
using TaskManager.Core.Files.Exceptions;
using TaskManager.Core.Files.Repositories;
using TaskManager.Core.Files.Services;
using TaskManager.Domain.Files.Enums;
using TaskManager.Shared;

namespace TaskManager.Application.Files.Commands.UploadFile;

public sealed class UploadFileHandler : IRequestHandler<UploadFile, UploadFileResponse>
{
    private readonly IFileRepository _fileRepository;
    private readonly IFileStorageService _fileStorageService;

    public UploadFileHandler(IFileRepository fileRepository, IFileStorageService fileStorageService)
    {
        _fileRepository = fileRepository;
        _fileStorageService = fileStorageService;
    }
    
    public async Task<UploadFileResponse> Handle(UploadFile request, CancellationToken cancellationToken)
    {
        if (request.File.Length > Globals.MaxFileSize)
            throw new InvalidFileSizeException();

        var fileUploadResult = await _fileStorageService.UploadAsync(request.File, cancellationToken);

        var file = new SystemFile
        {
            TaskId = request.TaskId,
            Name = request.File.FileName,
            Type = FileType.NoData,
            TotalBytes = fileUploadResult.File.File.Length,
            Data = fileUploadResult.File.File,
            ContentType = fileUploadResult.File?.ContentType
        };

        var fileId = await _fileRepository.AddAsync(file, cancellationToken);

        return new UploadFileResponse(fileId);
    }
}