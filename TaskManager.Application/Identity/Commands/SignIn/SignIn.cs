using MediatR;
using TaskManager.Core.Identity.DTOs;

namespace TaskManager.Application.Identity.Commands.SignIn;

public sealed record SignIn(string? EmailOrUserName, string? Password) : IRequest<JsonWebToken>;