using MediatR;

namespace TaskManager.Application.TaskCategories.Queries.GetAllTaskCategories;

public sealed record GetAllTaskCategories : IRequest<GetAllTaskCategoriesResponse>;
