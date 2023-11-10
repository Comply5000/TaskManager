using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Emails.Services;
using TaskManager.Core.Identity.Entities;
using TaskManager.Core.Identity.Exceptions;
using TaskManager.Core.Identity.Services;
using TaskManager.Shared;

namespace TaskManager.Application.Emails.Events.ResetPasswordEmail;

public sealed class ResetPasswordEmailHandler : INotificationHandler<ResetPasswordEmail>
{
    private readonly IEmailSenderService _emailSenderService;
    private readonly IIdentityService _identityService;

    public ResetPasswordEmailHandler(IEmailSenderService emailSenderService, IIdentityService identityService)
    {
        _emailSenderService = emailSenderService;
        _identityService = identityService;
    }
    
    public async Task Handle(ResetPasswordEmail notification, CancellationToken cancellationToken)
    {
        var response = await _identityService.GeneratePasswordResetTokenAsync(notification.Email, cancellationToken);
        
        var link = SetUrl(response.Token, response.UserId);
        var message = "Click link bellow reset your password" + Environment.NewLine + link;
        
        await _emailSenderService.SendEmailAsync(notification.Email, "Reset password", message);
    }
    
    private string SetUrl(string token, Guid userId)
    {
        return $"{Globals.ApplicationUrl}/reset-password?token={token}&userId={userId}";
    }
}