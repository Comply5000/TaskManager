using MediatR;

namespace TaskManager.Application.Emails.Events.ConfirmAccountEmail;

public sealed record ConfirmAccountEmail(string Email, string Token, Guid UserId) : INotification;
