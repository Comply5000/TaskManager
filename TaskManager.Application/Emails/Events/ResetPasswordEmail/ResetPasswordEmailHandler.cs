using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Emails.Services;
using TaskManager.Core.Identity.Entities;
using TaskManager.Core.Identity.Exceptions;
using TaskManager.Shared;

namespace TaskManager.Application.Emails.Events.ResetPasswordEmail;

public sealed class ResetPasswordEmailHandler : INotificationHandler<ResetPasswordEmail>
{
    private readonly IEmailSenderService _emailSenderService;
    private readonly UserManager<User> _userManager;

    public ResetPasswordEmailHandler(IEmailSenderService emailSenderService, UserManager<User> userManager)
    {
        _emailSenderService = emailSenderService;
        _userManager = userManager;
    }
    
    public async Task Handle(ResetPasswordEmail notification, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users.AsNoTracking()
                       .Where(x => x.Email == notification.Email)
                       .FirstOrDefaultAsync(cancellationToken)
                   ?? throw new UserWithEmailDoesntExistException();

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        
        var link = SetUrl(token, user.Id);
        var message = "Click link bellow reset your password" + Environment.NewLine + link;
        
        await _emailSenderService.SendEmailAsync(notification.Email, "Reset password", message);
    }
    
    private string SetUrl(string token, Guid userId)
    {
        return $"{Globals.ApplicationUrl}/reset-password?token={token}&userId={userId}";
    }
}