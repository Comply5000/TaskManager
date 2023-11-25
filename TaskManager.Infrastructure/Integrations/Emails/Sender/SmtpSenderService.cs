using MailKit.Net.Smtp;
using MimeKit;
using TaskManager.Core.Emails.Services;
using TaskManager.Infrastructure.Integrations.Emails.Configuration;

namespace TaskManager.Infrastructure.Integrations.Emails.Sender;

public sealed class SmtpSenderService : IEmailSenderService
{
    private readonly IConfigurationSmtp _configurationSmtp;
    private SmtpConfig _emailConfig;

    public SmtpSenderService(IConfigurationSmtp configurationSmtp)
    {
        _configurationSmtp = configurationSmtp;
    }
    
    public async Task<SentEmailDTO> SendEmailAsync(string email, string subject, string textBody)
    {
        _emailConfig = _configurationSmtp.ReturnEmailConfiguration();
        
        var emailMessage = CreateEmailMessage(email, subject, textBody, _emailConfig);
        using var client = new SmtpClient();
        {
            await client.ConnectAsync(_emailConfig.SmtpUrl, _emailConfig.SmtpPort,false);
            await client.AuthenticateAsync(_emailConfig.SmtpLogin, _emailConfig.SmtpPassword);
            var status = await client.SendAsync(emailMessage);
            await client.DisconnectAsync(true);
            return new SentEmailDTO(email);
        }
    }
    
    private MimeMessage CreateEmailMessage(string email, string subject, string textBody, SmtpConfig smtpConfig)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress(smtpConfig.SmtpSenderName, smtpConfig.SmtpSenderMail));
        emailMessage.To.Add(new MailboxAddress("", email));
        emailMessage.Subject = subject;
        
        var bodyBuilder = new BodyBuilder();
        bodyBuilder.TextBody = textBody;
        emailMessage.Body = bodyBuilder.ToMessageBody();

        return emailMessage;
    }
    
}