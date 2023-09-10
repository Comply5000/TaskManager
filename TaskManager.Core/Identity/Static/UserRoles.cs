using TaskManager.Core.Identity.Entities;

namespace TaskManager.Core.Identity.Static;

public static class UserRoles
{
    public const string User = nameof(User);
    public const string Admin = nameof(Admin);
    
    private static List<Role> Roles;

    static UserRoles()
    {
        Roles = new List<Role>()
        {
            new(Admin),
            new(User)
        };
    }

    public static List<Role> Get()
    {
        return Roles;
    }
}