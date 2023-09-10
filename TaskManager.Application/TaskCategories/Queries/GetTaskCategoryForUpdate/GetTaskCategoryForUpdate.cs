using MediatR;

namespace TaskManager.Application.TaskCategories.Queries.GetTaskCategoryForUpdate;

public sealed record GetTaskCategoryForUpdate(Guid Id) : IRequest<GetTaskCategoryForUpdateResponse>;
