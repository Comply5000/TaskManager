using TaskStatus = TaskManager.Core.Tasks.Enums.TaskStatus;

namespace TaskManager.Application.Tasks.DTOs;

public class TaskForUpdateDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public TaskStatus? Status { get; set; }
    public DateTimeOffset? Deadline { get; set; }
    public Guid? CategoryId { get; set; }
}