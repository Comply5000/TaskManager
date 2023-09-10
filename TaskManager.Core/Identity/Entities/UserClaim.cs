using Microsoft.AspNetCore.Identity;

namespace TaskManager.Core.Identity.Entities;

public sealed class UserClaim : IdentityUserClaim<Guid>
{
    public User User { get; set; }
}