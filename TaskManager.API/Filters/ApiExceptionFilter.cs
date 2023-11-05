using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TaskManager.Core.Identity.Exceptions;
using TaskManager.Infrastructure.EF.Context;
using TaskManager.Shared.Abstractions.Exceptions;
using TaskManager.Shared.Exceptions;

namespace TaskManager.API.Filters;

public sealed class ApiExceptionFilter : ExceptionFilterAttribute
{
    private readonly IDictionary<Type, Action<ExceptionContext>> _exceptionHandlers;

    public ApiExceptionFilter()
    {
        _exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
        {
            { typeof(OwnValidationException), HandleValidationException },
            { typeof(IdentityErrorException), HandleIdentityException },
            { typeof(UserLockedOutException), HandleUserLockoutException },
            { typeof(CreateUserException), HandleCreateUserException },
            { typeof(ChangePasswordException), HandleChangePasswordException },
            { typeof(ForbiddenAccessException), HandleForbiddenAccessException }
        };
    }
    
    public override void OnException(ExceptionContext context)
    {
        HandleException(context);

        base.OnException(context);
    }
    
    private void HandleException(ExceptionContext context)
    {
        var type = context.Exception.GetType();
        if (_exceptionHandlers.ContainsKey(type))
        {
            _exceptionHandlers[type].Invoke(context);
            return;
        }

        if (!context.ModelState.IsValid)
        {
            HandleInvalidModelStateException(context);
            return;
        }

        if (context.Exception is TaskManagerException)
        {
            HandleNetCoreTemplateException(context);
            return;
        }
        
        HandleUnknownException(context);
    }
    
    private void HandleNetCoreTemplateException(ExceptionContext context)
    {
        var exception = context.Exception as TaskManagerException;

        var details = new ProblemDetails()
        {
            Title = exception?.Message
        };

        context.Result = new BadRequestObjectResult(details);

        context.ExceptionHandled = true;
    }

    private void HandleForbiddenAccessException(ExceptionContext context)
    {
        context.Result = new ForbidResult();

        context.ExceptionHandled = true;
    }

    private void HandleValidationException(ExceptionContext context)
    {
        var exception = context.Exception as OwnValidationException;

        var details = new ValidationProblemDetails(exception?.Errors);

        context.Result = new BadRequestObjectResult(details);

        context.ExceptionHandled = true;
    }

    private void HandleIdentityException(ExceptionContext context)
    {
        var exception = context.Exception as IdentityErrorException;

        var details = new
        {
            Title = exception.Message,
            Status = 400,
            exception.Errors
        };

        context.Result = new BadRequestObjectResult(details);

        context.ExceptionHandled = true;
    }

    private void HandleUserLockoutException(ExceptionContext context)
    {
        var exception = context.Exception as UserLockedOutException;

        var details = new
        {
            Title = "Account lockout",
            Status = 400,
            exception.ReasonWhy,
            exception.LockoutEnd
        };

        context.Result = new BadRequestObjectResult(details);

        context.ExceptionHandled = true;
    }

    private void HandleCreateUserException(ExceptionContext context)
    {
        var exception = context.Exception as CreateUserException;


        var details = new ValidationProblemDetails(exception?.Errors)
        {
            Title = exception?.Message
        };

        context.Result = new BadRequestObjectResult(details);

        context.ExceptionHandled = true;
    }
    
    private void HandleChangePasswordException(ExceptionContext context)
    {
        var exception = context.Exception as ChangePasswordException;
        
        var details = new ValidationProblemDetails(exception?.Errors)
        {
            Title = exception?.Message
        };

        context.Result = new BadRequestObjectResult(details);

        context.ExceptionHandled = true;
    }

    private void HandleInvalidModelStateException(ExceptionContext context)
    {
        var details = new ValidationProblemDetails(context.ModelState);

        context.Result = new BadRequestObjectResult(details);

        context.ExceptionHandled = true;
    }
    
    private static void HandleUnknownException(ExceptionContext context)
    {
        var details = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "An error occurred while processing your request.",
        };
        
        // var isInDevRoles = context.HttpContext.User.IsInRole(UserRoles.Dev);
        
        // if (isInDevRoles)
        {
            details.Detail = $"{context.Exception.Message} {context.Exception.Source} {context.Exception.StackTrace}";
        } 
        
        context.Result = new ObjectResult(details)
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };

        context.ExceptionHandled = true;
    }
}