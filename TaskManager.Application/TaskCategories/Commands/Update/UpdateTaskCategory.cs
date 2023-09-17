using System.Text.Json.Serialization;
using MediatR;
using TaskManager.Shared.Responses;

namespace TaskManager.Application.TaskCategories.Commands.Update;

public sealed record UpdateTaskCategory(string? Name, string? Description, string? PageUrl) : IRequest<CreateOrUpdateResponse>
{
    [JsonIgnore] public Guid Id { get; set; }
}
