using MediatR;

namespace TaskManager.Application.TaskCategories.Commands.Delete;

public sealed record DeleteTaskCategory(Guid Id) : IRequest;