using TaskManager.Core.Tasks.Entities;
using TaskManager.Domain.Files.Enums;
using TaskManager.Shared.Abstractions.Entities;

namespace TaskManager.Core.Files.Entities;

public class SystemFile : BaseEntity
{
    public string Name { get; set; }
    public string ContentType { get; set; }
    public FileType Type { get; set; }
    public byte[] Data { get; set; }
    public long TotalBytes { get; set; }
    
    public Guid? TaskId { get; set; }
    public TaskModel Task { get; set; }
}