using MediatR;
using TaskManager.Shared.Models;
using TaskStatus = TaskManager.Core.Tasks.Enums.TaskStatus;

namespace TaskManager.Application.Tasks.Queries.GetTasks;

public sealed record GetTasks(
    string? Search,
    Guid? CategoryId,
    TaskStatus? Status,
    TaskOrderBy? OrderBy,
    bool IsOrderByDesc = false) : PaginationRequest, IRequest<GetTasksResponse>;

public enum TaskOrderBy
{
    Deadline = 1,
    CreatedAt = 2,
    LastModifiedAt = 3,
}