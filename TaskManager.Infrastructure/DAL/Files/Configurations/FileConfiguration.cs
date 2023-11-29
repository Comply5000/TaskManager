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
    private readonly EFContext _context;

    public FileConfiguration(EFContext context)
    {
        _context = context;
    }
    
    public void Configure(EntityTypeBuilder<SystemFile> builder)
    {
        builder.HasQueryFilter(x => x.EntryStatus != EntryStatus.Deleted);
    }
}