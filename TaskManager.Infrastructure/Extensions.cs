﻿using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TaskManager.Application.Shared.Common.Identity;
using TaskManager.Core.Identity.Services;
using TaskManager.Core.Shared.Services;
using TaskManager.Core.TaskCategories.Repositories;
using TaskManager.Infrastructure.EF.Context;
using TaskManager.Infrastructure.EF.Identity.Services;
using TaskManager.Infrastructure.EF.Shared.Services;
using TaskManager.Infrastructure.EF.TaskCategories.Repositories;

namespace TaskManager.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(cfg=>cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IDateService, DateService>();
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<ITokenService, TokenService>();
        
        services.AddScoped<ITaskCategoriesRepository, TaskCategoriesRepository>();
        
        services.AddDbContext<EFContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DatabaseConnection")!,
                opt =>
                {
                    opt.CommandTimeout(30);
                    opt.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                }).LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name }, LogLevel.Information);
        });
        
        return services;
    }
}