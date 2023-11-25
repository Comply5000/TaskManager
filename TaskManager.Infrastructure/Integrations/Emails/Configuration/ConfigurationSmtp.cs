using Microsoft.Extensions.Configuration;

namespace TaskManager.Infrastructure.Integrations.Emails.Configuration;

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