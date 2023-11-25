using MediatR;
using TaskManager.Shared.Responses;

namespace TaskManager.Application.Identity.Commands.SignUp;

public sealed record SignUp(string? UserName, string? Email, string? Password, string? ConfirmedPassword) : IRequest;