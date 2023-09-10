using MediatR;

namespace TaskManager.Application.Files.Queries.DownloadFile;

public sealed record DownloadFile(Guid FileId) : IRequest<DownloadFileResponse>;
