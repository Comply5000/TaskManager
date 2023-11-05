using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Files.Entities;
using TaskManager.Core.Identity.Entities;
using TaskManager.Core.Identity.Services;
using TaskManager.Core.TaskCategories.Entities;
using TaskManager.Core.Tasks.Entities;
using TaskManager.Infrastructure.EF.Files.Configurations;
using TaskManager.Infrastructure.EF.Identity.Configurations;
using TaskManager.Infrastructure.EF.TaskCategories.Configurations;
using TaskManager.Infrastructure.EF.Tasks.Configurations;
using TaskManager.Shared.Abstractions.Entities;
using TaskManager.Shared.Abstractions.Enums;

namespace TaskManager.Infrastructure.EF.Context;

public class EFContext : IdentityDbContext<User, Role, Guid, UserClaim, UserRole, IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
{
    public DbSet<TaskModel> Tasks { get; set; }
    public DbSet<TaskCategory> TaskCategories { get; set; }
    public DbSet<SystemFile> Files { get; set; }

    private readonly Guid _userId = Guid.Empty;
    
    public EFContext(DbContextOptions<EFContext> options) : base(options) {}
    
    public EFContext(DbContextOptions<EFContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
    {
        //_userId = httpContextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        _ = Guid.TryParse(httpContextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier),
            out _userId);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // foreach (var entity in modelBuilder.Model.GetEntityTypes())
        // {
        //     foreach (var property in entity.GetProperties())
        //     {
        //         if (property.ClrType == typeof(string))
        //         {
        //             property.SetMaxLength(250);
        //         }
        //     }
        // }

        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
        modelBuilder.ApplyConfiguration(new TaskConfiguration(_userId));
        modelBuilder.ApplyConfiguration(new TaskCategoryConfiguration(_userId));
        modelBuilder.ApplyConfiguration(new FileConfiguration(_userId));
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        foreach (var entry in ChangeTracker.Entries())
        {
            if(entry.Entity is BaseEntity)
                switch (entry.State)
                {
                    case EntityState.Added:
                        if (entry.CurrentValues["CreatedById"] is Guid createdById && createdById == Guid.Empty)
                            entry.CurrentValues["CreatedById"] = _userId;
                        if (entry.CurrentValues["CreatedAt"] is DateTimeOffset createdAt && createdAt == DateTimeOffset.MinValue)
                            entry.CurrentValues["CreatedAt"] = DateTimeOffset.UtcNow;
                        entry.CurrentValues["EntryStatus"] = EntryStatus.Active;
                        break;

                    case EntityState.Modified:
                        entry.CurrentValues["LastModifiedById"] = _userId;
                        entry.CurrentValues["LastModifiedAt"] = DateTimeOffset.UtcNow;
                        break;

                    case EntityState.Deleted:
                        entry.CurrentValues["LastModifiedById"] = _userId;
                        entry.CurrentValues["LastModifiedAt"] = DateTimeOffset.UtcNow;
                        entry.CurrentValues["InactivatedById"] = _userId;
                        entry.CurrentValues["InactivatedAt"] = DateTimeOffset.UtcNow;
                        entry.CurrentValues["EntryStatus"] = EntryStatus.Deleted;
                        entry.State = EntityState.Modified;
                        break;
                }
        }

        return base.SaveChangesAsync(cancellationToken);
    }

    public Task<int> SaveChangesBaseAsync(CancellationToken cancellationToken = new())
    {
        return base.SaveChangesAsync(cancellationToken);
    }
}