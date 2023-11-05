using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManager.Application.Tasks.Queries.GetTask;
using TaskManager.Core.Shared.Services;
using TaskManager.Infrastructure.EF.Context;

namespace TaskManager.Infrastructure.EF.Tasks.Queries.GetTaskHandler;

public sealed class GetTaskHandler : IRequestHandler<GetTask, GetTaskResponse?>
{
    private readonly EFContext _context;
    private readonly IDateService _dateService;

    public GetTaskHandler(EFContext context, IDateService dateService)
    {
        _context = context;
        _dateService = dateService;
    }
    
    public async Task<GetTaskResponse?> Handle(GetTask request, CancellationToken cancellationToken)
    {
        var currentDate = _dateService.CurrentDate();
        
        var todoTask = await _context.Tasks.AsNoTracking()
            .Where(x => x.Id == request.Id)
            .Include(x => x.Category)
            .Include(x => x.Files)
            .Select(x => new GetTaskResponse(x.AsTaskDto(currentDate)))
            .FirstOrDefaultAsync(cancellationToken);

        return todoTask;
    }
}