using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Identity.Entities;
using TaskManager.Core.Identity.Exceptions;

namespace TaskManager.Application.Identity.Commands.ConfirmAccount;

public sealed class ConfirmAccountHandler : IRequestHandler<ConfirmAccount>
{
    private readonly UserManager<User> _userManager;

    public ConfirmAccountHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }
    
    public async Task Handle(ConfirmAccount request, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken)
                   ?? throw new ConfirmAccountException();

        var result = await _userManager.ConfirmEmailAsync(user, request.Token);
        if (!result.Succeeded)
            throw new ConfirmAccountException();
    }
}