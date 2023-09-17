using TaskManager.Core.Common.DTOs;

namespace TaskManager.Application.Files.DTOs;

public sealed class FileDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; } 
    public string? ContentType { get; set; }
    public FileSizeDto? Size { get; set; }
    public BaseEnum? Type { get; set; }
}