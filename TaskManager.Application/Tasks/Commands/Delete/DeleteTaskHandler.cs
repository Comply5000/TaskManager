using MediatR;
using TaskManager.Core.Tasks.Exceptions;
using TaskManager.Core.Tasks.Repositories;

namespace TaskManager.Application.Tasks.Commands.Delete;

public sealed class DeleteTaskHandler : IRequestHandler<DeleteTask>
{
    private readonly ITasksRepository _tasksRepository;

    public DeleteTaskHandler(ITasksRepository tasksRepository)
    {
        _tasksRepository = tasksRepository;
    }
    
    public async Task Handle(DeleteTask request, CancellationToken cancellationToken)
    {
        var todoTask = await _tasksRepository.GetAsync(request.Id, cancellationToken) 
                         ?? throw new TaskNotFoundException();

        await _tasksRepository.DeleteAsync(todoTask, cancellationToken);
    }
}