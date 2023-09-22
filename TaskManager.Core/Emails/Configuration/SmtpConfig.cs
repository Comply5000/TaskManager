﻿namespace TaskManager.Core.Emails.Configuration;

public class SmtpConfig
{
    public string SmtpUrl { get; set; }
    public int SmtpPort { get; set; }
    public string SmtpLogin { get; set; }
    public string SmtpPassword { get; set; }
    public string SmtpSenderMail { get; set; }
}