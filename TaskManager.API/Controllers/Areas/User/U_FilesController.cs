using Microsoft.AspNetCore.Mvc;
using TaskManager.API.Attributes;
using TaskManager.Application.Files.Commands.DeleteFile;
using TaskManager.Application.Files.Commands.UploadFile;
using TaskManager.Application.Files.Queries.GetFile;
using TaskManager.Application.Files.Queries.GetTaskFiles;
using TaskManager.Core.Files.Services;
using TaskManager.Core.Identity.Static;
using TaskManager.Shared.Responses;


namespace TaskManager.API.Controllers.Areas.User;

[Route($"{Endpoints.BaseUrl}/files")]
[ApiAuthorize(Roles = UserRoles.User)]
public sealed class U_FilesController : BaseController
{
    /// <summary>
    /// Add file to Task
    /// </summary>
    [HttpPost("{taskId:guid}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CreateOrUpdateResponse>> CreateAssignment([FromRoute] Guid taskId, IFormFile file, CancellationToken cancellationToken)
    {
        var request = new UploadFile(file, taskId);
        var result = await Mediator.Send(request, cancellationToken);
        return Created(string.Empty, result);
    }
    
    
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteFile(Guid id, CancellationToken cancellationToken)
    {
        await Mediator.Send(new DeleteFile(id), cancellationToken);
        return Ok();
    }
    
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetFile(Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetFile(id), cancellationToken);
        return Ok(result);
    }
    
    [HttpGet("{taskId:guid}/download-all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetFiles(Guid taskId, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetTaskFiles(taskId), cancellationToken);
        return File(result.Content, result.ContentType, result.FileName);
    }
}