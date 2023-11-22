using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Core.Identity.Services;
using TaskManager.Core.TaskCategories.Entities;
using TaskManager.Infrastructure.EF.Context;
using TaskManager.Shared.Abstractions.Enums;

namespace TaskManager.Infrastructure.EF.TaskCategories.Configurations;

public sealed class TaskCategoryConfiguration : IEntityTypeConfiguration<TaskCategory>
{
    private readonly EFContext _context;

    public TaskCategoryConfiguration(EFContext context)
    {
        _context = context;
    }
    
    public void Configure(EntityTypeBuilder<TaskCategory> builder)
    {
        builder.HasQueryFilter(x => x.EntryStatus != EntryStatus.Deleted && x.CreatedById == _context._userId);
    }
}