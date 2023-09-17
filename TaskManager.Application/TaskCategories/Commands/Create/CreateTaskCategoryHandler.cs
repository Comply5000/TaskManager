using MediatR;
using TaskManager.Application.TaskCategories.Queries.CheckIfCategoryWithNameExists;
using TaskManager.Core.TaskCategories.Entities;
using TaskManager.Core.TaskCategories.Exceptions;
using TaskManager.Core.TaskCategories.Repositories;
using TaskManager.Shared.Responses;

namespace TaskManager.Application.TaskCategories.Commands.Create;

public sealed class CreateTaskCategoryHandler : IRequestHandler<CreateTaskCategory, CreateOrUpdateResponse>
{
    private readonly ITaskCategoriesRepository _taskCategoriesRepository;
    private readonly IMediator _mediator;

    public CreateTaskCategoryHandler(ITaskCategoriesRepository taskCategoriesRepository, IMediator mediator)
    {
        _taskCategoriesRepository = taskCategoriesRepository;
        _mediator = mediator;
    }
    
    public async Task<CreateOrUpdateResponse> Handle(CreateTaskCategory request, CancellationToken cancellationToken)
    {
        if (await _mediator.Send(new CheckIfCategoryWithNameExists(request.Name!), cancellationToken))
            throw new TaskCategoryWithNameExistsException();
        
        var todoTaskCategory = TaskCategory.Create(request.Name!, request.Description, request.PageUrl);

        var todoTaskCategoryId = await _taskCategoriesRepository.AddAsync(todoTaskCategory, cancellationToken);

        return new CreateOrUpdateResponse(todoTaskCategoryId);
    }
}