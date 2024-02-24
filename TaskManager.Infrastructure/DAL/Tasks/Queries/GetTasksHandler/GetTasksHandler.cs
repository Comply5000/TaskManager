using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManager.Application.Tasks.Queries.GetTasks;
using TaskManager.Core.Shared.Services;
using TaskManager.Infrastructure.DAL.EF.Context;
using TaskManager.Shared.Extensions;
using EFCore = Microsoft.EntityFrameworkCore.EF;

namespace TaskManager.Infrastructure.EF.Tasks.Queries.GetTasksHandler;

public sealed class GetTasksHandler : IRequestHandler<GetTasks, GetTasksResponse?>
{
    private readonly EFContext _context;
    private readonly IDateService _dateService;

    public GetTasksHandler(EFContext context, IDateService dateService)
    {
        _context = context;
        _dateService = dateService;
    }
    
    public async Task<GetTasksResponse?> Handle(GetTasks request, CancellationToken cancellationToken)
    {
        var query = _context.Tasks
            .Include(x => x.Category)
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Search))
            query = query.Where(v =>
                EFCore.Functions.ILike(v.Name, $"%{request.Search}%") ||
                EFCore.Functions.ILike(v.Category.Name, $"%{request.Search}%"));

        if (request.CategoryId is not null)
            query = query.Where(x => x.CategoryId == request.CategoryId);
        
        if (request.Status is not null)
            query = query.Where(x => x.Status == request.Status);
        
        if (request.Priority is not null)
            query = query.Where(x => x.Priority == request.Priority);

        if (request.OrderBy is not null)
        {
            switch (request.OrderBy)
            {
                case TaskOrderBy.Deadline:
                    query = request.IsOrderByDesc ? query.OrderByDescending(x => x.Deadline) : query.OrderBy(x => x.Deadline);
                    break;
                case TaskOrderBy.CreatedAt:
                    query = request.IsOrderByDesc ? query.OrderByDescending(x => x.CreatedAt) : query.OrderBy(x => x.CreatedAt);
                    break;
                case TaskOrderBy.LastModifiedAt:
                    query = request.IsOrderByDesc ? query.OrderByDescending(x => x.LastModifiedAt) : query.OrderBy(x => x.LastModifiedAt);
                    break;
            }
        }

        var currentDate = _dateService.CurrentOffsetDate();
        
        var tasks = await query
            .Select(x => x.AsTaskForListDto(currentDate))
            .PaginatedListAsync(request);

        return new GetTasksResponse(tasks);
    }
}