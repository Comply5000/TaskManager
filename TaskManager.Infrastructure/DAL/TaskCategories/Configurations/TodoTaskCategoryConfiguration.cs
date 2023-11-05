using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Core.Identity.Services;
using TaskManager.Core.TaskCategories.Entities;
using TaskManager.Infrastructure.EF.Context;
using TaskManager.Shared.Abstractions.Enums;

namespace TaskManager.Infrastructure.EF.TaskCategories.Configurations;

public sealed class TaskCategoryConfiguration : IEntityTypeConfiguration<TaskCategory>
{
    private readonly Guid _userId;

    public TaskCategoryConfiguration(Guid userId)
    {
        _userId = userId;
    }
    
    public void Configure(EntityTypeBuilder<TaskCategory> builder)
    {
        builder.HasQueryFilter(x => x.EntryStatus != EntryStatus.Deleted && x.CreatedById == _userId);
    }
}