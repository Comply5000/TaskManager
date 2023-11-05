using Microsoft.Extensions.Configuration;
using TaskManager.Core.Emails.Configuration;

namespace TaskManager.Infrastructure.Integrations.Email.Configuration;

public sealed class ConfigurationSmtp : IConfigurationSmtp
{
    private readonly SmtpConfig _configuration = new();
    
    public ConfigurationSmtp(IConfiguration configuration)
    {
        configuration.GetSection("SMTP").Bind(_configuration);
    }
    
    public SmtpConfig ReturnEmailConfiguration()
    {
        return _configuration;
    }
}