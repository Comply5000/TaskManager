using TaskManager.Application.Tasks.DTOs;
using TaskManager.Shared.Models;

namespace TaskManager.Application.Tasks.Queries.GetTasks;

public sealed record GetTasksResponse(PaginatedList<TaskForListDto> Tasks);
