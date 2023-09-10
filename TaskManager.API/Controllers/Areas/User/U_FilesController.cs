using Microsoft.AspNetCore.Mvc;
using TaskManager.API.Attributes;
using TaskManager.Core.Identity.Static;


namespace TaskManager.API.Controllers.Areas.User;

[Route("api/files")]
[ApiAuthorize(Roles = UserRoles.User)]
public sealed class U_FilesController : BaseController
{
    /// <summary>
    /// Add file to Task
    /// </summary>
    // [HttpPost("{todoTaskId:guid}")]
    // [ProducesResponseType(StatusCodes.Status201Created)]
    // [ProducesResponseType(StatusCodes.Status400BadRequest)]
    // public async Task<ActionResult<CreateOrUpdateResponse>> CreateAssignment([FromRoute] Guid todoTaskId, IFormFile file, CancellationToken cancellationToken)
    // {
    //     var request = new UploadFile(file, todoTaskId);
    //     var result = await Mediator.Send(request, cancellationToken);
    //     return Created(string.Empty, result);
    // }
    //
    //
    // [HttpDelete("{id:guid}")]
    // [ProducesResponseType(StatusCodes.Status201Created)]
    // [ProducesResponseType(StatusCodes.Status400BadRequest)]
    // public async Task<IActionResult> DeleteFile(Guid id, CancellationToken cancellationToken)
    // {
    //     await Mediator.Send(new DeleteFile(id), cancellationToken);
    //     return Ok();
    // }
    //
    // [HttpGet("{id:guid}")]
    // [ProducesResponseType(StatusCodes.Status201Created)]
    // [ProducesResponseType(StatusCodes.Status400BadRequest)]
    // public async Task<IActionResult> DownloadFile(Guid id, CancellationToken cancellationToken)
    // {
    //     var result = await Mediator.Send(new DownloadFile(id), cancellationToken);
    //     return File(result.Content, result.ContentType, result.FileName);
    // }
}