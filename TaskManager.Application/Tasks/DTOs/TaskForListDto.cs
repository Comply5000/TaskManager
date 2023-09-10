using TaskManager.Core.Common.DTOs;

namespace TaskManager.Application.Tasks.DTOs;

public sealed class TaskForListDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? CategoryName { get; set; }
    public BaseEnum? Status { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? LastModifiedAt { get; set; }
    public DateTimeOffset? Deadline { get; set; }
    public bool IsLessThenDay { get; set; }
}
