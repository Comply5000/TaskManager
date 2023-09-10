using MediatR;

namespace TaskManager.Application.Tasks.Queries.GetTodoTaskForUpdate;

public sealed record GetTaskForUpdate(Guid Id) : IRequest<GetTaskForUpdateResponse>;