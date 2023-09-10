using TaskManager.Shared.Abstractions.Enums;

namespace TaskManager.Shared.Abstractions.Entities;

public abstract class BaseEntity
{
    public Guid Id { get; set; }
    public EntryStatus EntryStatus { get; set; }
    
    public Guid CreatedById { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    
    public Guid LastModifiedById { get; set; }
    public DateTimeOffset? LastModifiedAt { get; set; }
    
    public Guid InactivatedById { get; set; }
    public DateTimeOffset? InactivatedAt { get; set; }
}