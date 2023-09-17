using TaskManager.Application.Files.DTOs;
using TaskManager.Application.TaskCategories.DTOs;
using TaskManager.Core.Common.DTOs;

namespace TaskManager.Application.Tasks.DTOs;

public sealed class TaskDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public BaseEnum? Status { get; set; }
    public BaseEnum? Priority { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? LastModifiedAt { get; set; }
    public DateTimeOffset? Deadline { get; set; }
    public bool IsLessThenDay { get; set; }
    public TaskCategoryDto? Category { get; set; }
    public List<FileDto> Files { get; set; }
    
}