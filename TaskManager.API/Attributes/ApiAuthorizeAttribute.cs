using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace TaskManager.API.Attributes;

public sealed class ApiAuthorizeAttribute : AuthorizeAttribute
{
    public ApiAuthorizeAttribute(params string[] roles) : base()
    {
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme;
        // Dołączamy role, oddzielając je przecinkami
        Roles = string.Join(",", roles);
    }
}