using TaskManager.Shared.Abstractions.Exceptions;

namespace TaskManager.Core.TaskCategories.Exceptions;

public sealed class TaskCategoryNotFoundException : TaskManagerException
{
    public TaskCategoryNotFoundException() : base("Category not found") { }
}