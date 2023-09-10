using Microsoft.EntityFrameworkCore;
using TaskManager.Core.TaskCategories.Entities;
using TaskManager.Core.TaskCategories.Repositories;
using TaskManager.Infrastructure.EF.Context;

namespace TaskManager.Infrastructure.EF.TaskCategories.Repositories;

public sealed class TaskCategoriesRepository : ITaskCategoriesRepository
{
    private readonly EFContext _context;
    private readonly DbSet<TaskCategory> _taskCategories;

    public TaskCategoriesRepository(EFContext context)
    {
        _context = context;
        _taskCategories = context.TaskCategories;
    }

    public async Task<TaskCategory?> GetAsync(Guid id, CancellationToken cancellationToken)
        => await _taskCategories
            .Include(x => x.Tasks)
            .ThenInclude(x => x.Files)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<Guid> AddAsync(TaskCategory taskCategory, CancellationToken cancellationToken)
    {
        var result = await _taskCategories.AddAsync(taskCategory, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return result.Entity.Id;
    }

    public async Task<Guid> UpdateAsync(TaskCategory taskCategory, CancellationToken cancellationToken)
    {
        var result = _taskCategories.Update(taskCategory);

        await _context.SaveChangesAsync(cancellationToken);

        return result.Entity.Id;
    }

    public async Task DeleteAsync(TaskCategory taskCategory, CancellationToken cancellationToken)
    {
        _taskCategories.Remove(taskCategory);
        await _context.SaveChangesAsync(cancellationToken);
    }
}