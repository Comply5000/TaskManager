using MediatR;
using TaskManager.Application.TaskCategories.Queries.CheckIfCategoryWithNameExists;
using TaskManager.Core.TaskCategories.Exceptions;
using TaskManager.Core.TaskCategories.Repositories;
using TaskManager.Shared.Responses;

namespace TaskManager.Application.TaskCategories.Commands.Update;

public sealed class UpdateTaskCategoryHandler : IRequestHandler<UpdateTaskCategory, CreateOrUpdateResponse>
{
    private readonly ITaskCategoriesRepository _taskCategoriesRepository;
    private readonly IMediator _mediator;

    public UpdateTaskCategoryHandler(ITaskCategoriesRepository taskCategoriesRepository, IMediator mediator)
    {
        _taskCategoriesRepository = taskCategoriesRepository;
        _mediator = mediator;
    }
    
    public async Task<CreateOrUpdateResponse> Handle(UpdateTaskCategory request, CancellationToken cancellationToken)
    {
        if (await _mediator.Send(new CheckIfCategoryWithNameExists(request.Name!, request.Id), cancellationToken))
            throw new TaskCategoryWithNameExistsException();
        
        var todoTaskCategory = await _taskCategoriesRepository.GetAsync(request.Id, cancellationToken)
                               ?? throw new TaskCategoryNotFoundException();

        todoTaskCategory.Update(request.Name!, request.Description);

        var todoTaskCategoryId = await _taskCategoriesRepository.UpdateAsync(todoTaskCategory, cancellationToken);

        return new CreateOrUpdateResponse(todoTaskCategoryId);
    }
}