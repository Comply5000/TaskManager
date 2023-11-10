namespace TaskManager.Core.Identity.DTOs;

public sealed class ResetPasswordTokenDto
{
    public string Token { get; set; }
    public Guid UserId { get; set; }
}