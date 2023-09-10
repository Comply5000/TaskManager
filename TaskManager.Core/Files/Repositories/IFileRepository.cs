using TaskManager.Core.Files.Entities;

namespace TaskManager.Core.Files.Repositories;

public interface IFileRepository
{
    Task<Guid> AddAsync(SystemFile file, CancellationToken cancellationToken);
    Task<SystemFile?> GetAsync(Guid id, CancellationToken cancellationToken);
    Task DeleteAsync(SystemFile file, CancellationToken cancellationToken);
}