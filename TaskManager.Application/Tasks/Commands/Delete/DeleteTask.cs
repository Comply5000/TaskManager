using MediatR;

namespace TaskManager.Application.Tasks.Commands.Delete;

public sealed record DeleteTask(Guid Id) : IRequest;
