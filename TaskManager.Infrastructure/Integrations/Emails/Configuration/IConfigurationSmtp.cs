namespace TaskManager.Infrastructure.Integrations.Emails.Configuration;

public interface IConfigurationSmtp
{
    SmtpConfig ReturnEmailConfiguration();
}