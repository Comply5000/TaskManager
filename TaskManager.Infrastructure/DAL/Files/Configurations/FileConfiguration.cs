using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Core.Files.Entities;
using TaskManager.Infrastructure.EF.Context;
using TaskManager.Shared.Abstractions.Enums;

namespace TaskManager.Infrastructure.DAL.Files.Configurations;

public sealed class FileConfiguration : IEntityTypeConfiguration<SystemFile>
{
    private readonly EFContext _context;

    public FileConfiguration(EFContext context)
    {
        _context = context;
    }
    
    public void Configure(EntityTypeBuilder<SystemFile> builder)
    {
        builder.HasQueryFilter(x => x.EntryStatus != EntryStatus.Deleted && x.CreatedById == _context._userId);
    }
}