using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Tasks.Entities;
using TaskManager.Core.Tasks.Repositories;
using TaskManager.Infrastructure.DAL.EF.Context;

namespace TaskManager.Infrastructure.EF.Tasks.Repositories;

public sealed class TasksRepository : ITasksRepository
{
    private readonly EFContext _context;
    private readonly DbSet<TaskModel> _tasks;

    public TasksRepository(EFContext context)
    {
        _context = context;
        _tasks = context.Tasks;
    }

    public async Task<TaskModel?> GetAsync(Guid id, CancellationToken cancellationToken)
        => await _tasks.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<Guid> AddAsync(TaskModel task, CancellationToken cancellationToken)
    {
        var result = await _tasks.AddAsync(task, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return result.Entity.Id;
    }

    public async Task<Guid> UpdateAsync(TaskModel task, CancellationToken cancellationToken)
    {
        var result = _tasks.Update(task);

        await _context.SaveChangesAsync(cancellationToken);

        return result.Entity.Id;
    }

    public async Task DeleteAsync(TaskModel task, CancellationToken cancellationToken)
    {
        _tasks.Remove(task);
        await _context.SaveChangesAsync(cancellationToken);
    }
}