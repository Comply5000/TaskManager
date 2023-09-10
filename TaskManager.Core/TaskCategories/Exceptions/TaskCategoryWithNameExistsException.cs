using TaskManager.Shared.Abstractions.Exceptions;

namespace TaskManager.Core.TaskCategories.Exceptions;

public sealed class TaskCategoryWithNameExistsException : TaskManagerException
{
    public TaskCategoryWithNameExistsException() : base("Category with that name already exists")
    {
    }
}