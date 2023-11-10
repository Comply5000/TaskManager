using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManager.Application.Shared.Common.Identity;
using TaskManager.Core.Identity.DTOs;
using TaskManager.Core.Identity.Entities;
using TaskManager.Core.Identity.Exceptions;
using TaskManager.Core.Identity.Services;
using TaskManager.Shared.Exceptions;

namespace TaskManager.Application.Identity.Commands.SignIn;

public sealed class SignInHandler : IRequestHandler<SignIn, JsonWebToken>
{
    private readonly IIdentityService _identityService;

    public SignInHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }
    
    public async Task<JsonWebToken> Handle(SignIn request, CancellationToken cancellationToken)
    {
        return await _identityService.SignIn(request.EmailOrUserName, request.Password, cancellationToken);
    }
}