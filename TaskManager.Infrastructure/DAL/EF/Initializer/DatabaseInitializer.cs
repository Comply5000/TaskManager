using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TaskManager.Infrastructure.DAL.EF.Context;
using TaskManager.Infrastructure.DAL.EF.Initializer.UserRoles;

namespace TaskManager.Infrastructure.DAL.EF.Initializer;

internal sealed class DatabaseInitializer : IHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public DatabaseInitializer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetService(typeof(EFContext)) as EFContext;
        var roleManager = scope.ServiceProvider.GetService(typeof(RoleManager<IdentityRole<Guid>>)) as RoleManager<IdentityRole<Guid>>;

        if (context is not null)
        {
            await context.Database.MigrateAsync(cancellationToken);
            
            await UserRolesSeeder.SeedAsync(roleManager, context, cancellationToken);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}