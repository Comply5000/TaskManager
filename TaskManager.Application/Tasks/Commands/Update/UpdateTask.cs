﻿using System.Text.Json.Serialization;
using MediatR;
using TaskManager.Shared.Responses;
using TaskStatus = TaskManager.Core.Tasks.Enums.TaskStatus;

namespace TaskManager.Application.Tasks.Commands.Update;

public sealed record UpdateTask(
    string? Name,
    string? Description,
    DateTimeOffset? Deadline,
    TaskStatus? Status,
    Guid CategoryId) : IRequest<CreateOrUpdateResponse>
{
    [JsonIgnore] public Guid Id { get; set; }
}