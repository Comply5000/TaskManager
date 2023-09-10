using TaskManager.Application.Files.DTOs;
using TaskManager.Core.Files.Entities;

namespace TaskManager.Infrastructure.EF.Files.Queries;

internal static class Extensions
{
    public static FileDto AsFileDto(this SystemFile file)
    {
        return new()
        {
            Id = file.Id,
            Name = file.Name,
            ContentType = file.ContentType,
            TotalSize = file.TotalBytes,
            Type = file.Type
        };
    }
}