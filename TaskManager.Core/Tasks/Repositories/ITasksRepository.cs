using TaskManager.Core.Tasks.Entities;

namespace TaskManager.Core.Tasks.Repositories;

public interface ITasksRepository
{
    Task<TaskModel?> GetAsync(Guid id, CancellationToken cancellationToken);
    Task<Guid> AddAsync(TaskModel task, CancellationToken cancellationToken);
    Task<Guid> UpdateAsync(TaskModel task, CancellationToken cancellationToken);
    Task DeleteAsync(TaskModel task, CancellationToken cancellationToken);
}