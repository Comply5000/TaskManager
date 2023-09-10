using TaskManager.Application.Tasks.DTOs;

namespace TaskManager.Application.Tasks.Queries.GetTask;

public sealed record GetTaskResponse(TaskDto? Task);
