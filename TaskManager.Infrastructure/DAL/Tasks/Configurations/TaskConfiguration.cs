using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Core.Identity.Services;
using TaskManager.Core.Tasks.Entities;
using TaskManager.Infrastructure.EF.Context;
using TaskManager.Shared.Abstractions.Enums;

namespace TaskManager.Infrastructure.EF.Tasks.Configurations;

public class TaskConfiguration : IEntityTypeConfiguration<TaskModel>
{
    private readonly EFContext _context;

    public TaskConfiguration(EFContext context)
    {
        _context = context;
    }
    
    public void Configure(EntityTypeBuilder<TaskModel> builder)
    {
        builder
            .HasMany(x => x.Files)
            .WithOne(x => x.Task)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasQueryFilter(x => x.EntryStatus != EntryStatus.Deleted && x.CreatedById == _context._userId);
    }
}