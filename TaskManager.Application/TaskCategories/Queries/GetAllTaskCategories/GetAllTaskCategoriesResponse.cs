using TaskManager.Application.TaskCategories.DTOs;

namespace TaskManager.Application.TaskCategories.Queries.GetAllTaskCategories;

public sealed record GetAllTaskCategoriesResponse(List<TaskCategoryDto> TaskCategories);
