using TaskManager.Application.Files.DTOs;
using TaskManager.Core.Files.Entities;
using TaskManager.Infrastructure.EF.Files.Queries.Static;

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
            Size = FileSize.Count(file.TotalBytes),
            Type = file.Type
        };
    }
}