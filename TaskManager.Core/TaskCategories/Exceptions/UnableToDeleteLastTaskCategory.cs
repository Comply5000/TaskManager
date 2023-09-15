using TaskManager.Shared.Abstractions.Exceptions;

namespace TaskManager.Core.TaskCategories.Exceptions;

public sealed class UnableToDeleteLastTaskCategory : TaskManagerException
{
    public UnableToDeleteLastTaskCategory() : base("You can't delete last category") { }
}