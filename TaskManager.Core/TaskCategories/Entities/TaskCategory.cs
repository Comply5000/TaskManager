#nullable enable
using TaskManager.Core.Tasks.Entities;
using TaskManager.Shared.Abstractions.Entities;

namespace TaskManager.Core.TaskCategories.Entities;

public sealed class TaskCategory : BaseEntity
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? PageUrl { get; set; }

    public List<TaskModel> Tasks { get; set; }

    private TaskCategory() {}

    private TaskCategory(string name, string? description, string? pageUrl)
    {
        Name = name;
        Description = description;
        PageUrl = pageUrl;
    }

    public static TaskCategory Create(string name, string? description, string? pageUrl)
        => new(name, description, pageUrl);

    public TaskCategory Update(string name, string? description, string? pageUrl)
    {
        Name = name;
        Description = description;
        PageUrl = pageUrl;

        return this;
    }
}