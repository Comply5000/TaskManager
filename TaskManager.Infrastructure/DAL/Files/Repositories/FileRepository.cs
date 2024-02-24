using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Files.Entities;
using TaskManager.Core.Files.Repositories;
using TaskManager.Infrastructure.DAL.EF.Context;

namespace TaskManager.Infrastructure.EF.Files.Repositories;

public sealed class FileRepository : IFileRepository
{
    private readonly EFContext _context;
    private readonly DbSet<SystemFile> _files;

    public FileRepository(EFContext context)
    {
        _context = context;
        _files = context.Files;
    }
    
    public async Task<Guid> AddAsync(SystemFile file, CancellationToken cancellationToken)
    {
        var result = await _files.AddAsync(file, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return result.Entity.Id;
    }

    public async Task<SystemFile?> GetAsync(Guid id, CancellationToken cancellationToken)
        => await _files.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task DeleteAsync(SystemFile file, CancellationToken cancellationToken)
    {
        _files.Remove(file);
        await _context.SaveChangesAsync(cancellationToken);
    }
}