using MediatR;
using Microsoft.AspNetCore.Http;

namespace TaskManager.Application.Files.Commands.UploadFile;

public sealed record UploadFile(IFormFile File, Guid TaskId) : IRequest<UploadFileResponse>;