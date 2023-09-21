using MediatR;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TaskManager.Application.Emails.Events.ConfirmAccount;

public sealed record ConfirmAccount(string Email, string Token, Guid UserId) : INotification;
