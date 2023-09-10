using Microsoft.AspNetCore.Identity;

namespace TaskManager.Core.Identity.Entities;

public sealed class UserRole : IdentityUserRole<Guid>
{
    public User User { get; set; }
    public Role Role { get; set; }
}