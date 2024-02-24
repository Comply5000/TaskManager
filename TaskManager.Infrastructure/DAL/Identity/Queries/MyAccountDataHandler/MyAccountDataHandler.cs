using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManager.Application.Identity.Queries.MyAccountData;
using TaskManager.Core.Identity.Services;
using TaskManager.Infrastructure.DAL.EF.Context;

namespace TaskManager.Infrastructure.EF.Identity.Queries.MyAccountDataHandler;

public sealed class MyAccountDataHandler : IRequestHandler<MyAccountData, MyAccountDataResponse?>
{
    private readonly EFContext _context;
    private readonly ICurrentUserService _currentUserService;

    public MyAccountDataHandler(EFContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }
    
    public async Task<MyAccountDataResponse?> Handle(MyAccountData request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        
        var myAccountData = await _context.Users.AsNoTracking()
            .Where(x => x.Id == userId)
            .Select(x => x.AsMyAccountDataDto())
            .FirstOrDefaultAsync(cancellationToken);

        if (myAccountData is null)
            return null;
        
        var userFilesSize = await _context.Files
            .Where(x => x.CreatedById == userId)
            .SumAsync(x => x.TotalBytes, cancellationToken);
        
        myAccountData.FilesSize = Math.Round(userFilesSize / 1048576.0, 1);

        return new MyAccountDataResponse(myAccountData);

    }
}