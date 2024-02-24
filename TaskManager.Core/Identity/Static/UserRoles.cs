using Microsoft.AspNetCore.Identity;
using TaskManager.Core.Identity.Entities;

namespace TaskManager.Core.Identity.Static;

public static class UserRoles
{
    public const string User = nameof(User);
    public const string PremiumUser = nameof(PremiumUser);
    
    private static List<IdentityRole<Guid>> Roles;

    static UserRoles()
    {
        Roles = new List<IdentityRole<Guid>>()
        {
            new(PremiumUser),
            new(User)
        };
    }

    public static List<IdentityRole<Guid>> Get()
    {
        return Roles;
    }
}