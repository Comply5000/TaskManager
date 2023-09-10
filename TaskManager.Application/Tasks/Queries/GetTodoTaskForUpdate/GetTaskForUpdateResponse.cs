using TaskManager.Application.Tasks.DTOs;

namespace TaskManager.Application.Tasks.Queries.GetTodoTaskForUpdate;

public sealed record GetTaskForUpdateResponse(TaskForUpdateDto? Task);
