namespace TaskManager.Core.Files.DTOs;

public record FileUploadResponseDto(bool Success, string? Message = null, FileResponse? File = null);

