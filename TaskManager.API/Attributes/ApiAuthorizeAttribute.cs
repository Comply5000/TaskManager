using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace TaskManager.API.Attributes;

public sealed class ApiAuthorizeAttribute : AuthorizeAttribute
{
    public ApiAuthorizeAttribute() : base()
    {
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme;
    }
}