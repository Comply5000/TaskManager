using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.Tasks.Queries.GetTasks;
using TaskManager.Core.Common.DTOs;
using TaskManager.Core.Common.Extensions;
using TaskStatus = TaskManager.Core.Tasks.Enums.TaskStatus;

namespace TaskManager.API.Controllers.Areas.Globals;

[Route($"{Endpoints.BaseUrl}/enums")]
public sealed class EnumsController : BaseController
{
    
    [HttpGet("task-status")]
    public ActionResult<List<BaseEnum>> GetTodoTaskStatusEnum()
        => Ok(EnumExtensions.GetValues<TaskStatus>());
    
    [HttpGet("task-order-by")]
    public ActionResult<List<BaseEnum>> GetTodoTaskOrderByEnum()
        => Ok(EnumExtensions.GetValues<TaskOrderBy>());
}