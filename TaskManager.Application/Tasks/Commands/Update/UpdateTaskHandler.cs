using MediatR;
using TaskManager.Core.Tasks.Exceptions;
using TaskManager.Core.Tasks.Repositories;
using TaskManager.Shared.Responses;

namespace TaskManager.Application.Tasks.Commands.Update;

public sealed class UpdateTaskHandler : IRequestHandler<UpdateTask, CreateOrUpdateResponse>
{
    private readonly ITasksRepository _tasksRepository;

    public UpdateTaskHandler(ITasksRepository tasksRepository)
    {
        _tasksRepository = tasksRepository;
    }
    
    public async Task<CreateOrUpdateResponse> Handle(UpdateTask request, CancellationToken cancellationToken)
    {
        var todoTask= await _tasksRepository.GetAsync(request.Id, cancellationToken) 
                         ?? throw new TaskNotFoundException();

        todoTask.Update(
            request.Name,
            request.Description,
            request.Deadline,
            request.Status,
            (Guid)request.CategoryId!);

        var todoTaskId = await _tasksRepository.UpdateAsync(todoTask, cancellationToken);

        return new CreateOrUpdateResponse(todoTaskId);
    }
}