namespace TaskManager.Core.Files.DTOs;

public record FileResponse(byte[] File, string Name, string ContentType);