namespace TaskManager.Application.Files.Queries.DownloadFile;

public sealed record DownloadFileResponse(string FileName, string ContentType, Stream Content);
