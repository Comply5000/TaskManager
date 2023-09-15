using MediatR;
using TaskManager.Shared.Responses;

namespace TaskManager.Application.TaskCategories.Commands.Create;

public sealed record CreateTaskCategory(string? Name, string? Description) : IRequest<CreateOrUpdateResponse>;
