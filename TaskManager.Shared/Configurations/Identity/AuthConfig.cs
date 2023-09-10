namespace TaskManager.Shared.Configurations.Identity;

public sealed record AuthConfig
{
    public string JwtKey { get; init; }
    public string JwtIssuer { get; init; }
    public TimeSpan Expires { get; init; }
    public uint RefreshTokenTTL { get; init; }
}