using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace TaskManager.Application.Files.Queries.GetFile;

public sealed record GetFile(Guid FileId) : IRequest<FileContentResult>;
