using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Identity.Entities;
using TaskManager.Core.Identity.Exceptions;
using TaskManager.Core.Identity.Services;

namespace TaskManager.Application.Identity.Commands.ConfirmAccount;

public sealed class ConfirmAccountHandler : IRequestHandler<ConfirmAccount>
{
    private readonly IIdentityService _identityService;

    public ConfirmAccountHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }
    
    public async Task Handle(ConfirmAccount request, CancellationToken cancellationToken)
    {
        await _identityService.ConfirmAccount(request.UserId, request.Token, cancellationToken);
    }
}