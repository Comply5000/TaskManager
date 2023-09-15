using TaskManager.Core.TaskCategories.Entities;

namespace TaskManager.Core.TaskCategories.Repositories;

public interface ITaskCategoriesRepository
{
    Task<TaskCategory> GetAsync(Guid id, CancellationToken cancellationToken);
    Task<Guid> AddAsync(TaskCategory taskCategory, CancellationToken cancellationToken);
    Task<Guid> UpdateAsync(TaskCategory taskCategory, CancellationToken cancellationToken);
    Task DeleteAsync(TaskCategory taskCategory, CancellationToken cancellationToken);
    Task<int> CountAsync(CancellationToken cancellationToken);

}