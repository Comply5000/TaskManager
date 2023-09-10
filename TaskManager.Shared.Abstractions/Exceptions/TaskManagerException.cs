namespace TaskManager.Shared.Abstractions.Exceptions;

public abstract class TaskManagerException : Exception
{
    protected TaskManagerException(string message) : base(message) { }
}