using MediatR;
using TaskManager.Core.Files.Entities;
using TaskManager.Core.Tasks.Entities;
using TaskManager.Core.Tasks.Repositories;
using TaskManager.Shared.Responses;

namespace TaskManager.Application.Tasks.Commands.Create;

public sealed class CreateTaskHandler : IRequestHandler<CreateTask, CreateOrUpdateResponse>
{
    private readonly ITasksRepository _tasksRepository;
    private readonly IMediator _mediator;

    public CreateTaskHandler(ITasksRepository tasksRepository, IMediator mediator)
    {
        _tasksRepository = tasksRepository;
        _mediator = mediator;
    }
    
    public async Task<CreateOrUpdateResponse> Handle(CreateTask request, CancellationToken cancellationToken)
    {
        var todoTask = TaskModel.Create(
            request.Name,
            request.Description,
            request.Deadline,
            request.Status,
            request.CategoryId);
        
        var todoTaskId = await _tasksRepository.AddAsync(todoTask, cancellationToken);

        return new CreateOrUpdateResponse(todoTaskId);
    }
}