using TaskManager.Core.Emails.Configuration;

namespace TaskManager.Infrastructure.Integrations.Email.Configuration;

public interface IConfigurationSmtp
{
    SmtpConfig ReturnEmailConfiguration();
}