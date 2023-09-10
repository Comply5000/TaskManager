#nullable enable
using TaskManager.Core.Tasks.Entities;
using TaskManager.Shared.Abstractions.Entities;

namespace TaskManager.Core.TaskCategories.Entities;

public sealed class TaskCategory : BaseEntity
{
    public string Name { get; set; }
    public string? Description { get; set; }

    public List<TaskModel> Tasks { get; set; }

    private TaskCategory() {}

    private TaskCategory(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public static TaskCategory Create(string name, string description)
        => new(name, description);

    public TaskCategory Update(string name, string description)
    {
        Name = name;
        Description = description;

        return this;
    }
}