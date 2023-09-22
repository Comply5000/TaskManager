using MediatR;
using TaskManager.Core.Emails.Services;
using TaskManager.Shared;

namespace TaskManager.Application.Emails.Events.ConfirmAccountEmail;

public sealed class ConfirmAccountEmailHandler : INotificationHandler<ConfirmAccountEmail>
{
    private readonly IEmailSenderService _emailSenderService;

    public ConfirmAccountEmailHandler(IEmailSenderService emailSenderService)
    {
        _emailSenderService = emailSenderService;
    }
    
    public async Task Handle(Events.ConfirmAccountEmail.ConfirmAccountEmail notification, CancellationToken cancellationToken)
    {
        var link = SetUrl(notification);
        var message = "Click link bellow to activate your account" + Environment.NewLine + link;
        
        await _emailSenderService.SendEmailAsync(notification.Email, "Activate your account", message);
    }

    private string SetUrl(Events.ConfirmAccountEmail.ConfirmAccountEmail notification)
    {
        return $"{Globals.ApplicationUrl}/confirm-account?token={notification.Token}&userId={notification.UserId}";
    }
}