using TaskManager.Shared.Abstractions.Exceptions;

namespace TaskManager.Core.Tasks.Exceptions;

public sealed class TaskNotFoundException : TaskManagerException
{
    public TaskNotFoundException() : base("Task not found") { }
}