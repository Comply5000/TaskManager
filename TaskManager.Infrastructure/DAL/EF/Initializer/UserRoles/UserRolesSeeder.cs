﻿using Microsoft.AspNetCore.Identity;
using TaskManager.Infrastructure.DAL.EF.Context;

namespace TaskManager.Infrastructure.DAL.EF.Initializer.UserRoles;

internal static class UserRolesSeeder
{
    internal static async Task SeedAsync(RoleManager<IdentityRole<Guid>> roleManager, EFContext context, CancellationToken cancellationToken)
    {
        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        
        var roles = Core.Identity.Static.UserRoles.Get();
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role.Name))
            {
                await roleManager.CreateAsync(role);
            }
        }

        await transaction.CommitAsync(cancellationToken);
    }
    
}