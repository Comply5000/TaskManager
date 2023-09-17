using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.Tasks.Queries.GetTasks;
using TaskManager.Core.Common.DTOs;
using TaskManager.Core.Common.Extensions;
using TaskManager.Core.Tasks.Enums;
using TaskStatus = TaskManager.Core.Tasks.Enums.TaskStatus;

namespace TaskManager.API.Controllers.Areas.Globals;

[Route($"{Endpoints.BaseUrl}/enums")]
public sealed class EnumsController : BaseController
{
    
    [HttpGet("task-status")]
    public ActionResult<List<BaseEnum>> GetTaskStatusEnum()
        => Ok(EnumExtensions.GetValues<TaskStatus>());
    
    [HttpGet("task-priority")]
    public ActionResult<List<BaseEnum>> GetTaskPriorityEnum()
        => Ok(EnumExtensions.GetValues<TaskPriority>());
    
    [HttpGet("task-order-by")]
    public ActionResult<List<BaseEnum>> GetTaskOrderByEnum()
        => Ok(EnumExtensions.GetValues<TaskOrderBy>());
}