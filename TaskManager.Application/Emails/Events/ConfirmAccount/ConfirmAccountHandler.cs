using MediatR;
using TaskManager.Core.Emails.Services;
using TaskManager.Shared;

namespace TaskManager.Application.Emails.Events.ConfirmAccount;

public sealed class ConfirmAccountHandler : INotificationHandler<ConfirmAccount>
{
    private readonly IEmailSenderService _emailSenderService;

    public ConfirmAccountHandler(IEmailSenderService emailSenderService)
    {
        _emailSenderService = emailSenderService;
    }
    
    public async Task Handle(ConfirmAccount notification, CancellationToken cancellationToken)
    {
        var link = SetUrl(notification);
        var message = "Click link bellow to activate your account" + Environment.NewLine + link;
        
        await _emailSenderService.SendEmailAsync(notification.Email, "Activate your account", message);
    }

    private string SetUrl(ConfirmAccount notification)
    {
        return $"{Globals.ApplicationUrl}/confirm-account?token={notification.Token}&userId={notification.UserId}";
    }
}