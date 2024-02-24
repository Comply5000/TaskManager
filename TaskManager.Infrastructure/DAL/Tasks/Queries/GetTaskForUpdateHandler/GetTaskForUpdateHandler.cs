using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManager.Application.Tasks.Queries.GetTodoTaskForUpdate;
using TaskManager.Infrastructure.DAL.EF.Context;

namespace TaskManager.Infrastructure.EF.Tasks.Queries.GetTaskForUpdateHandler;

public sealed class GetTaskForUpdateHandler : IRequestHandler<GetTaskForUpdate, GetTaskForUpdateResponse?>
{
    private readonly EFContext _context;

    public GetTaskForUpdateHandler(EFContext context)
    {
        _context = context;
    }
    
    public async Task<GetTaskForUpdateResponse?> Handle(GetTaskForUpdate request, CancellationToken cancellationToken)
    {
        var todoTask = await _context.Tasks.AsNoTracking()
            .Where(x => x.Id == request.Id)
            .Select(x => new GetTaskForUpdateResponse(x.AsTaskForUpdateDto()))
            .FirstOrDefaultAsync(cancellationToken);

        return todoTask;
    }
}