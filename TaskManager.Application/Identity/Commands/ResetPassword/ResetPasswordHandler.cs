using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManager.Application.Emails.Events.ResetPasswordEmail;
using TaskManager.Core.Identity.Entities;
using TaskManager.Core.Identity.Exceptions;
using TaskManager.Core.Identity.Services;
using TaskManager.Shared.Exceptions;

namespace TaskManager.Application.Identity.Commands.ResetPassword;

public sealed class ResetPasswordHandler : IRequestHandler<ResetPassword>
{
    private readonly IIdentityService _identityService;

    public ResetPasswordHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }
    
    public async Task Handle(ResetPassword request, CancellationToken cancellationToken)
    {
        await _identityService.ResetPassword(request.UserId, request.Token, request.Password, cancellationToken);
    }
}