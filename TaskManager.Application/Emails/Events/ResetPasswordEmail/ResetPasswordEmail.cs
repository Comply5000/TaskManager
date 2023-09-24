using MediatR;

namespace TaskManager.Application.Emails.Events.ResetPasswordEmail;

public sealed record ResetPasswordEmail(string Email) : INotification;
