using MediatR;
using TaskManager.Core.Identity.DTOs;

namespace TaskManager.Application.Identity.Commands.RefreshToken;

public sealed record RefreshTokenCommand(string? Token) : IRequest<JsonWebToken>;