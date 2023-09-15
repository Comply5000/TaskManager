using MediatR;
using Microsoft.AspNetCore.Http;
using TaskManager.Core.Common.Requests;
using TaskManager.Shared.Responses;
using TaskStatus = TaskManager.Core.Tasks.Enums.TaskStatus;

namespace TaskManager.Application.Tasks.Commands.Create;

public sealed record CreateTask(
    string? Name,
    string? Description,
    DateTimeOffset? Deadline,
    TaskStatus? Status,
    Guid CategoryId,
    IFormFileCollection? Files) : IRequest<CreateOrUpdateResponse>, ITransactionalRequest;
