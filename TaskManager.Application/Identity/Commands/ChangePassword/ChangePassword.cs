using MediatR;

namespace TaskManager.Application.Identity.Commands.ChangePassword;

public sealed record ChangePassword(
    string? CurrentPassword,
    string? NewPassword,
    string? NewPasswordConfirmed) : IRequest;
