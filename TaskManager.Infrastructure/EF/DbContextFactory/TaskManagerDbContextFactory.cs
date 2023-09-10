using Microsoft.EntityFrameworkCore;
using TaskManager.Infrastructure.EF.Context;

namespace TaskManager.Infrastructure.EF.DbContextFactory;

public class TaskManagerDbContextFactory : DesignTimeDbContextFactoryBase<EFContext>
{
    protected override Context.EFContext CreateNewInstance(DbContextOptions<EFContext> options)
    {
        return new Context.EFContext(options);
    }
}