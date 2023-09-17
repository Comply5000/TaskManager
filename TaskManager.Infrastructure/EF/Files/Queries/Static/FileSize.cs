using TaskManager.Application.Files.DTOs;

namespace TaskManager.Infrastructure.EF.Files.Queries.Static;

public static class FileSize
{
    private const double KB = 1024.0;
    private const double MB = 1048576.0;
    private const double GB = 1073741824.0;
    
    public static FileSizeDto Count(long size)
    {
        if (size < KB)
            return new FileSizeDto(size, "B");

        if (size < MB)
            return new FileSizeDto(Math.Round(size / KB, 1), "KB");

        if (size < GB)
            return new FileSizeDto(Math.Round(size / MB, 1), "MB");

        return new FileSizeDto(0, string.Empty);
    }
}