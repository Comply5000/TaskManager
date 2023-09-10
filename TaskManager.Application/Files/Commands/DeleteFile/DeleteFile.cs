using MediatR;

namespace TaskManager.Application.Files.Commands.DeleteFile;

public sealed record DeleteFile(Guid Id) : IRequest;
