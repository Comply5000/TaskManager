using MediatR;
using TaskManager.Core.Files.Repositories;

namespace TaskManager.Application.Files.Commands.DeleteFile;

public sealed class DeleteFileHandler : IRequestHandler<DeleteFile>
{
    private readonly IFileRepository _fileRepository;

    public DeleteFileHandler(IFileRepository fileRepository)
    {
        _fileRepository = fileRepository;
    }
    
    public async Task Handle(DeleteFile request, CancellationToken cancellationToken)
    {
        var file = await _fileRepository.GetAsync(request.Id, cancellationToken)
                   ?? throw new FileNotFoundException();

        await _fileRepository.DeleteAsync(file, cancellationToken);
    }
}