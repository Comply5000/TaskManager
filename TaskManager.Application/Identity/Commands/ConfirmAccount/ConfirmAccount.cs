using MediatR;

namespace TaskManager.Application.Identity.Commands.ConfirmAccount;

public sealed record ConfirmAccount(string Token, Guid UserId) : IRequest;
