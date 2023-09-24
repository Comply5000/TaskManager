using MediatR;

namespace TaskManager.Application.Identity.Commands.ResetPassword;

public sealed record ResetPassword(string Token, Guid UserId, string? Password, string? ConfirmedPassword) : IRequest;
