using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Core.Files.Entities;
using TaskManager.Core.Identity.Services;
using TaskManager.Core.TaskCategories.Entities;
using TaskManager.Infrastructure.EF.Context;
using TaskManager.Shared.Abstractions.Enums;

namespace TaskManager.Infrastructure.EF.Files.Configurations;

public sealed class FileConfiguration : IEntityTypeConfiguration<SystemFile>
{
    private readonly Guid _userId;

    public FileConfiguration(Guid userId)
    {
        _userId = userId;
    }
    
    public void Configure(EntityTypeBuilder<SystemFile> builder)
    {
        builder.HasQueryFilter(x => x.EntryStatus != EntryStatus.Deleted && x.CreatedById == _userId);
    }
}