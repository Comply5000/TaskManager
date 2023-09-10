using Microsoft.AspNetCore.Identity;

namespace TaskManager.Core.Identity.Entities;

public sealed class Role : IdentityRole<Guid>
{
    public Role()
    {
    }

    public Role(string roleName)
        : base(roleName)
    {
    }


    public IEnumerable<UserRole> UserRoles { get; } = new List<UserRole>();
}