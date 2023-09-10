// using Microsoft.AspNetCore.Mvc;
// using TaskManager.API.Attributes;
// using TaskManager.Core.Identity.Static;
//
//
// namespace TaskManager.API.Controllers.Areas.User;
//
// [Route("api/todo-tasks")]
// [ApiAuthorize(Roles = UserRoles.User)]
// public sealed class U_TasksController : BaseController
// {
//     /// <summary>
//     /// Get paginated list of Todotasks
//     /// </summary>
//     [HttpGet]
//     [ProducesResponseType(StatusCodes.Status200OK)]
//     [ProducesResponseType(StatusCodes.Status400BadRequest)]
//     public async Task<ActionResult<GetTodoTasksResponse>> GetAssignments([FromQuery] GetTodoTasks query, CancellationToken cancellationToken)
//     {
//         var result = await Mediator.Send(query, cancellationToken);
//         return Ok(result);
//     }
//     
//     /// <summary>
//     /// Get Assignment by Id
//     /// </summary>
//     [HttpGet("{id:guid}")]
//     [ProducesResponseType(StatusCodes.Status200OK)]
//     [ProducesResponseType(StatusCodes.Status400BadRequest)]
//     [ProducesResponseType(StatusCodes.Status404NotFound)]
//     public async Task<ActionResult<GetTodoTaskResponse>> GetAssignment(Guid id, CancellationToken cancellationToken)
//     {
//         var result = await Mediator.Send(new GetTodoTask(id), cancellationToken);
//         return OkOrNotFound(result);
//     }
//     
//     /// <summary>
//     /// Create new Task
//     /// </summary>
//     [HttpPost]
//     [ProducesResponseType(StatusCodes.Status201Created)]
//     [ProducesResponseType(StatusCodes.Status400BadRequest)]
//     public async Task<ActionResult<CreateOrUpdateResponse>> CreateAssignment([FromForm] CreateTodoTask command, CancellationToken cancellationToken)
//     {
//         var result = await Mediator.Send(command, cancellationToken);
//         return Created(string.Empty, result);
//     }
//     
//     /// <summary>
//     /// Get Assignment for update by Id
//     /// </summary>
//     [HttpGet("{id:guid}/update")]
//     [ProducesResponseType(StatusCodes.Status200OK)]
//     [ProducesResponseType(StatusCodes.Status400BadRequest)]
//     [ProducesResponseType(StatusCodes.Status404NotFound)]
//     public async Task<ActionResult<GetTodoTaskForUpdateResponse>> GetAssignmentForUpdate(Guid id, CancellationToken cancellationToken)
//     {
//         var result = await Mediator.Send(new GetTodoTaskForUpdate(id), cancellationToken);
//         return OkOrNotFound(result);
//     }
//     
//     /// <summary>
//     /// Update new Task
//     /// </summary>
//     [HttpPut("{id:guid}")]
//     [ProducesResponseType(StatusCodes.Status201Created)]
//     [ProducesResponseType(StatusCodes.Status400BadRequest)]
//     public async Task<ActionResult<CreateOrUpdateResponse>> UpdateAssignment(Guid id, [FromBody] UpdateTodoTask command, CancellationToken cancellationToken)
//     {
//         command.Id = id;
//         var result = await Mediator.Send(command, cancellationToken);
//         return Ok(result);
//     }
//     
//     /// <summary>
//     /// Delete new Task
//     /// </summary>
//     [HttpDelete("{id:guid}")]
//     [ProducesResponseType(StatusCodes.Status201Created)]
//     [ProducesResponseType(StatusCodes.Status400BadRequest)]
//     public async Task<IActionResult> DeleteAssignment(Guid id, CancellationToken cancellationToken)
//     {
//         await Mediator.Send(new DeleteTodoTask(id), cancellationToken);
//         return Ok();
//     }
// }