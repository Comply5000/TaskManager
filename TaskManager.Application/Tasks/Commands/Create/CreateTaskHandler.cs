using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TaskManager.Application.Files.Commands.UploadFile;
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
        var task = TaskModel.Create(
            request.Name!,
            request.Description,
            request.Deadline,
            request.Status,
            request.CategoryId);
        
        var taskId = await _tasksRepository.AddAsync(task, cancellationToken);

        await UploadFiles(request.Files, taskId, cancellationToken);

        return new CreateOrUpdateResponse(taskId);
    }

    private async Task UploadFiles(IFormFileCollection? files, Guid taskId, CancellationToken cancellationToken)
    {
        if (files is not null)
            foreach (var file in files)
            {
                await _mediator.Send(new UploadFile(file, taskId), cancellationToken);
            }
    }
    
}