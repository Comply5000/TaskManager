using Microsoft.AspNetCore.Mvc;
using TaskManager.API.Attributes;
using TaskManager.Application.Tasks.Commands.Create;
using TaskManager.Application.Tasks.Commands.Delete;
using TaskManager.Application.Tasks.Commands.Update;
using TaskManager.Application.Tasks.Queries.GetTask;
using TaskManager.Application.Tasks.Queries.GetTasks;
using TaskManager.Application.Tasks.Queries.GetTodoTaskForUpdate;
using TaskManager.Core.Identity.Static;
using TaskManager.Shared.Responses;


namespace TaskManager.API.Controllers.Areas.User;

[Route($"{Endpoints.BaseUrl}/tasks")]
[ApiAuthorize(Roles = UserRoles.User)]
public sealed class U_TasksController : BaseController
{
    /// <summary>
    /// Get paginated list of Tasks
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<GetTasksResponse>> GetTasks([FromQuery] GetTasks query, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(query, cancellationToken);
        return Ok(result);
    }
    
    /// <summary>
    /// Get Task by Id
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GetTaskResponse>> GetTask(Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetTask(id), cancellationToken);
        return OkOrNotFound(result);
    }
    
    /// <summary>
    /// Create new Task
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CreateOrUpdateResponse>> CreateTask([FromForm] CreateTask command, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(command, cancellationToken);
        return Created(string.Empty, result);
    }
    
    /// <summary>
    /// Get Task for update by Id
    /// </summary>
    [HttpGet("{id:guid}/update")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GetTaskForUpdateResponse>> GetTaskForUpdate(Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetTaskForUpdate(id), cancellationToken);
        return OkOrNotFound(result);
    }
    
    /// <summary>
    /// Update Task
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CreateOrUpdateResponse>> UpdateTask(Guid id, [FromBody] UpdateTask command, CancellationToken cancellationToken)
    {
        command.Id = id;
        var result = await Mediator.Send(command, cancellationToken);
        return Ok(result);
    }
    
    /// <summary>
    /// Delete Task
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteTask(Guid id, CancellationToken cancellationToken)
    {
        await Mediator.Send(new DeleteTask(id), cancellationToken);
        return Ok();
    }
}