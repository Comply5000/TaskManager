using MediatR;
using TaskManager.Core.TaskCategories.Exceptions;
using TaskManager.Core.TaskCategories.Repositories;

namespace TaskManager.Application.TaskCategories.Commands.Delete;

public sealed class DeleteTaskCategoryHandler : IRequestHandler<DeleteTaskCategory>
{
    private readonly ITaskCategoriesRepository _taskCategoriesRepository;

    public DeleteTaskCategoryHandler(ITaskCategoriesRepository taskCategoriesRepository)
    {
        _taskCategoriesRepository = taskCategoriesRepository;
    }
    
    public async Task Handle(DeleteTaskCategory request, CancellationToken cancellationToken)
    {
        var todoTaskCategory = await _taskCategoriesRepository.GetAsync(request.Id, cancellationToken)
                               ?? throw new TaskCategoryNotFoundException();
        
        await _taskCategoriesRepository.DeleteAsync(todoTaskCategory, cancellationToken);
    }
}