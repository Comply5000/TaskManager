using TaskManager.Application.TaskCategories.DTOs;
using TaskManager.Core.TaskCategories.Entities;

namespace TaskManager.Infrastructure.EF.TaskCategories.Queries;

internal static class Extensions
{
    public static TaskCategoryDto AsTaskCategoryDto(this TaskCategory taskCategory)
    {
        return new()
        {
            Id = taskCategory.Id,
            Name = taskCategory.Name,
            Description = taskCategory.Description,
            PageUrl = taskCategory.PageUrl
        };
    }
}