namespace TaskManager.Core.Emails.Services;

public interface IEmailSenderService
{
    Task<SentEmailDTO> SendEmailAsync(string email, string subject, string textBody);
}

public record SentEmailDTO(string SenderMail);