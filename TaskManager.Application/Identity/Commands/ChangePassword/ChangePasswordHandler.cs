using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Identity.Entities;
using TaskManager.Core.Identity.Exceptions;
using TaskManager.Core.Identity.Services;

namespace TaskManager.Application.Identity.Commands.ChangePassword;

public sealed class ChangePasswordHandler : IRequestHandler<ChangePassword>
{
    private readonly UserManager<User> _userManager;
    private readonly ICurrentUserService _currentUserService;

    public ChangePasswordHandler(UserManager<User> userManager, ICurrentUserService currentUserService)
    {
        _userManager = userManager;
        _currentUserService = currentUserService;
    }
    
    public async Task Handle(ChangePassword request, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users.Where(x => x.Id == _currentUserService.UserId)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new UserNotFoundException();
        
        var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, request.CurrentPassword!);

        if (isPasswordCorrect is false)
            throw new InvalidPasswordException();
        
        var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword!, request.NewPassword!);
        if (!result.Succeeded)
            throw new ChangePasswordException(result.Errors);
    }
}