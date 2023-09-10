#nullable enable
using TaskManager.Core.Files.Entities;
using TaskManager.Core.TaskCategories.Entities;
using TaskManager.Shared.Abstractions.Entities;
using TaskStatus = TaskManager.Core.Tasks.Enums.TaskStatus;

namespace TaskManager.Core.Tasks.Entities;

public sealed class TaskModel : BaseEntity
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public DateTimeOffset? Deadline { get; set; }
    public TaskStatus Status { get; set; }
    
    public Guid CategoryId { get; set; }
    public TaskCategory Category { get; set; }
    
    public List<SystemFile> Files { get; set; }

    private TaskModel(){}
    
    public TaskModel(string name, string description, DateTimeOffset? deadline, TaskStatus? status, Guid categoryId, List<SystemFile> files)
    {
        Name = name;
        Description = description;
        Deadline = deadline;
        Status = status ?? TaskStatus.New;
        CategoryId = categoryId;
        Files = files;
    }

    public static TaskModel Create(string name, string description, DateTimeOffset? deadline, TaskStatus? status,
        Guid categoryId, List<SystemFile> files)
        => new(name, description, deadline, status, categoryId, files);

    public TaskModel Update(string? name, string? description, DateTimeOffset? deadline, TaskStatus? status,
        Guid categoryId)
    {
        Name = name;
        Description = description;
        Deadline = deadline;
        Status = status ?? TaskStatus.NoData;
        CategoryId = categoryId;

        return this;
    }
}