using Microsoft.AspNetCore.Identity;
using TaskManager.Core.Identity.Enums;

namespace TaskManager.Core.Identity.Entities;

public sealed class User : IdentityUser<Guid>
{
    public UserStatus UserStatus { get; set; }
    public string RefreshToken { get; set; }
    public DateTimeOffset RefreshTokenExpires { get; set; }
    
}