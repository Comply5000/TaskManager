namespace TaskManager.Core.Identity.Services;

public interface ICurrentUserService
{
    string Email { get; }
    Guid UserId { get; }
}