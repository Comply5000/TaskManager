using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManager.Application.Emails.Events.ResetPasswordEmail;
using TaskManager.Core.Identity.Entities;
using TaskManager.Core.Identity.Exceptions;
using TaskManager.Shared.Exceptions;

namespace TaskManager.Application.Identity.Commands.ResetPassword;

public sealed class ResetPasswordHandler : IRequestHandler<ResetPassword>
{
    private readonly UserManager<User> _userManager;
    private readonly IMediator _mediator;

    public ResetPasswordHandler(UserManager<User> userManager, IMediator mediator)
    {
        _userManager = userManager;
        _mediator = mediator;
    }
    
    public async Task Handle(ResetPassword request, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken)
                   ?? throw new ResetPasswordException();

        var result = await _userManager.ResetPasswordAsync(user, request.Token, request.Password!);
        if (!result.Succeeded)
            throw new ChangePasswordException(result.Errors);
        
    }
}