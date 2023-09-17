using TaskManager.Application.Tasks.DTOs;
using TaskManager.Application.Tasks.Queries.GetTodoTaskForUpdate;
using TaskManager.Core.Tasks.Entities;
using TaskManager.Infrastructure.EF.Files.Queries;
using TaskManager.Infrastructure.EF.TaskCategories.Queries;
using TaskStatus = TaskManager.Core.Tasks.Enums.TaskStatus;

namespace TaskManager.Infrastructure.EF.Tasks.Queries;

internal static class Extensions
{
    public static TaskDto AsTaskDto(this TaskModel task, DateTimeOffset currentDate)
    {
        return new()
        {
            Id = task.Id,
            Name = task.Name,
            Description = task.Description,
            Status = task.Status,
            Priority = task.Priority,
            Deadline = task.Deadline,
            CreatedAt = task.CreatedAt,
            LastModifiedAt = task.LastModifiedAt,
            IsLessThenDay = (task.Deadline < currentDate.AddDays(1)) && task.Status != TaskStatus.Finished && task.Deadline != null,
            Category = task.Category.AsTaskCategoryDto(),
            Files = task.Files.Select(x => x.AsFileDto()).ToList()
        };
    }
    
    public static TaskForListDto AsTaskForListDto(this TaskModel task, DateTimeOffset currentDate)
    {
        return new()
        {
            Id = task.Id,
            Name = task.Name,
            CategoryName = task.Category?.Name,
            Status = task.Status,
            Priority = task.Priority,
            Deadline = task.Deadline,
            CreatedAt = task.CreatedAt,
            IsLessThenDay = (task.Deadline < currentDate.AddDays(1)) && task.Status != TaskStatus.Finished && task.Deadline != null,
        };
    }
    
    public static TaskForUpdateDto AsTaskForUpdateDto(this TaskModel task)
    {
        return new()
        {
            Id = task.Id,
            Name = task.Name,
            Description = task.Description,
            Status = task.Status,
            Priority = task.Priority,
            Deadline = task.Deadline,
            CategoryId = task.CategoryId
        };
    }
}