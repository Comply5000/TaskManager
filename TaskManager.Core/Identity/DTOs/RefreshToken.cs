namespace TaskManager.Core.Identity.DTOs;

public sealed class RefreshToken
{
    public string Token { get; set; }
    public DateTimeOffset Expires { get; set; }
}