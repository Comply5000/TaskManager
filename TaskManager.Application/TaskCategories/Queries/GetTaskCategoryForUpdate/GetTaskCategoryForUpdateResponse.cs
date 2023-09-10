using TaskManager.Application.TaskCategories.DTOs;

namespace TaskManager.Application.TaskCategories.Queries.GetTaskCategoryForUpdate;

public sealed record GetTaskCategoryForUpdateResponse(TaskCategoryDto? TaskCategory);
