using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Identity.Entities;
using TaskManager.Core.Identity.Exceptions;
using TaskManager.Core.Identity.Services;

namespace TaskManager.Application.Identity.Commands.ChangePassword;

public sealed class ChangePasswordHandler : IRequestHandler<ChangePassword>
{
    private readonly IIdentityService _identityService;

    public ChangePasswordHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }
    
    public async Task Handle(ChangePassword request, CancellationToken cancellationToken)
    {
        await _identityService.ChangePassword(request.CurrentPassword, request.NewPassword, cancellationToken);
    }
}