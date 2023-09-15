using Microsoft.AspNetCore.Mvc;
using TaskManager.API.Attributes;
using TaskManager.Application.TaskCategories.Commands.Create;
using TaskManager.Application.TaskCategories.Commands.Delete;
using TaskManager.Application.TaskCategories.Commands.Update;
using TaskManager.Application.TaskCategories.Queries.GetAllTaskCategories;
using TaskManager.Application.TaskCategories.Queries.GetTaskCategoryForUpdate;
using TaskManager.Core.Identity.Static;
using TaskManager.Shared.Responses;

namespace TaskManager.API.Controllers.Areas.User;

[Route($"{Endpoints.BaseUrl}/task-categories")]
[ApiAuthorize(Roles = UserRoles.User)]
public sealed class U_TaskCategoriesController : BaseController
{
    /// <summary>
    /// Get all list of Assignment Categories
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<GetAllTaskCategoriesResponse>> GetAssignments(CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetAllTaskCategories(), cancellationToken);
        return Ok(result);
    }
    
    /// <summary>
    /// Create new category
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CreateOrUpdateResponse>> CreateAssignment([FromBody] CreateTaskCategory command, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(command, cancellationToken);
        return Created(string.Empty, result);
    }
    
    /// <summary>
    /// Update category by Id
    /// </summary>
    [HttpPut("{id:Guid}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CreateOrUpdateResponse>>UpdateTodoTaskCategory([FromRoute] Guid id, [FromBody] UpdateTaskCategory command, CancellationToken cancellationToken)
    {
        command.Id = id;
        var result = await Mediator.Send(command, cancellationToken);
        return Created(string.Empty, result);
    }
    
    /// <summary>
    /// Get category by id for update
    /// </summary>
    [HttpGet("{id:guid}/update")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<GetTaskCategoryForUpdateResponse>> GetTodoTaskCategoryForUpdate([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetTaskCategoryForUpdate(id), cancellationToken);
        return Created(string.Empty, result);
    }
    
    /// <summary>
    /// delete category
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteAssignment([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await Mediator.Send(new DeleteTaskCategory(id), cancellationToken);
        return Ok();
    }
}