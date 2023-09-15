using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Core.Identity.Entities;
using TaskManager.Core.Identity.Static;

namespace TaskManager.API.Controllers.Areas.Dev;

[Route($"{Endpoints.BaseUrl}/seed")]
public class D_SeedDataController : BaseController
{
    private readonly RoleManager<Role> _roleManager;

    public D_SeedDataController(RoleManager<Role> roleManager)
    {
        _roleManager = roleManager;
    }

    [HttpPut("roles")]
    public async Task<IActionResult> CreateTest(CancellationToken cancellationToken)
    {
        var roles = UserRoles.Get();
        foreach (var role in roles)
        {
            if (!await _roleManager.RoleExistsAsync(role.Name))
            {
                await _roleManager.CreateAsync(role);
            }
        }

        return Ok();
    }
}
