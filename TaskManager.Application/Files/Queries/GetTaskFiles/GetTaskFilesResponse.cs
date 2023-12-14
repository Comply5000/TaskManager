namespace TaskManager.Application.Files.Queries.GetTaskFiles;

public sealed record GetTaskFilesResponse(MemoryStream Content, string ContentType, string FileName);