using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManager.Application.Shared.Common.Identity;
using TaskManager.Core.Identity.DTOs;
using TaskManager.Core.Identity.Entities;
using TaskManager.Core.Identity.Exceptions;
using TaskManager.Core.Identity.Services;
using TaskManager.Core.Identity.Static;
using TaskManager.Shared.Exceptions;

namespace TaskManager.Application.Identity.Commands.SignInGoogle;

public sealed class SignInGoogleHandler : IRequestHandler<SignInGoogleCommand, JsonWebToken>
{
    private readonly IIdentityService _identityService;

    public SignInGoogleHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }
    
    public async Task<JsonWebToken> Handle(SignInGoogleCommand request, CancellationToken cancellationToken)
    {
        return await _identityService.SignInGoogle(cancellationToken);
    }
}