using Microsoft.AspNetCore.Identity;
using TaskManager.Core.Identity.Enums;

namespace TaskManager.Core.Identity.Entities;

public sealed class User : IdentityUser<Guid>
{
    public UserStatus UserStatus { get; set; }
    
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public ICollection<UserClaim> UserClaims { get; set; } = new List<UserClaim>();
    
}