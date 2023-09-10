using TaskManager.Application.Common.DTOs;

namespace TaskManager.Application.Files.DTOs;

public class FileDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; } 
    public string? ContentType { get; set; }
    public decimal? TotalSize { get; set; }
    public BaseEnum? Type { get; set; }
}