﻿namespace TaskManager.Core.Identity.DTOs;

public class JsonWebToken
{
    public string AccessToken { get; init; }
    public RefreshToken RefreshToken { get; set; }
    public long Expires { get; init; }
    public Guid UserId { get; init; }
    public string Email { get; set; }
    public bool IsExternal { get; set; }
    public ICollection<string> Roles { get; init; } = new List<string>();
    public IDictionary<string, string> Claims { get; init; } = new Dictionary<string, string>();
}