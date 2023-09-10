using MediatR;

namespace TaskManager.Application.Tasks.Queries.GetTask;

public sealed record GetTask(Guid Id) : IRequest<GetTaskResponse>;