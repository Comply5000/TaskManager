using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using TaskManager.Core.Emails.Configuration;
using TaskManager.Core.Emails.Services;

namespace TaskManager.Infrastructure.EF.Emails.Services;

public sealed class SmtpSenderService : IEmailSenderService
{
    private readonly IConfiguration _configuration;

    public SmtpSenderService(IConfiguration configuration)
    {
        _configuration = configuration;
        
    }
    
    public async Task<SentEmailDTO> SendEmailAsync(string email, string subject, string textBody)
    {
        var smtpConfig = new SmtpConfig();
        _configuration.GetSection("SMTP").Bind(smtpConfig);
        
        var emailMessage = CreateEmailMessage(email, subject, textBody, smtpConfig.SmtpSenderMail);
        using var client = new SmtpClient();
        {
            await client.ConnectAsync(smtpConfig.SmtpUrl, smtpConfig.SmtpPort,false);
            await client.AuthenticateAsync(smtpConfig.SmtpLogin, smtpConfig.SmtpPassword);
            var status = await client.SendAsync(emailMessage);
            await client.DisconnectAsync(true);
            return new SentEmailDTO(email);
        }
    }
    
    private MimeMessage CreateEmailMessage(string email, string subject, string textBody, string sender)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress("", sender));
        emailMessage.To.Add(new MailboxAddress("", email));
        emailMessage.Subject = subject;
        
        var bodyBuilder = new BodyBuilder();
        bodyBuilder.TextBody = textBody;
        emailMessage.Body = bodyBuilder.ToMessageBody();

        return emailMessage;
    }
    
}