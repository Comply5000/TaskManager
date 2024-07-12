using MediatR;
using TaskManager.Core.Identity.DTOs;

namespace TaskManager.Application.Identity.Commands.SignInGoogle;

public sealed record SignInGoogleCommand : IRequest<JsonWebToken>;
