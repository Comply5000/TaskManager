using TaskManager.Application.Identity.DTOs;
using TaskManager.Application.Identity.Queries.MyAccountData;
using TaskManager.Core.Identity.Entities;
using TaskManager.Infrastructure.EF.Files.Queries.Static;

namespace TaskManager.Infrastructure.EF.Identity.Queries;

internal static class Extensions
{
    public static MyAccountDataDto AsMyAccountDataDto(this User user)
    {
        return new()
        {
            Email = user.Email,
            UserName = user.UserName
        };
    }
}