using MediatR;

namespace TaskManager.Application.Files.Queries.GetFile;

public sealed record GetFile(Guid FileId) : IRequest<GetFileResponse>;
