using MediatR;

namespace TaskManager.Application.Files.Queries.GetTaskFiles;

public sealed record GetTaskFiles(Guid TaskId) : IRequest<GetTaskFilesResponse>;
