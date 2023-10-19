using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.ComponentModel.DataAnnotations;

namespace Water.Common.AspNetCore.Filters;


/// <summary>
/// Application Layer에서 발생한 예외를 처리합니다.
/// </summary>
public class BaseExceptionFilterAttribute : ExceptionFilterAttribute
{
    protected IDictionary<Type, Action<ExceptionContext>> ExceptionHandlers { get; }

    public BaseExceptionFilterAttribute()
    {
        ExceptionHandlers = new Dictionary<Type, Action<ExceptionContext>> {
      //  { typeof(HandlerException), HandleHandlerException },
      //  { typeof(NotFoundException), HandleNotFoundException },
            { typeof(ValidationException), HandleValidationException },
      //  { typeof(AuthorizationException), HandleAuthorizationException }
        };
    }

    protected virtual void HandleValidationException(ExceptionContext context)
    {
        ValidationException exception = (ValidationException)context.Exception;
        string code = "0111";
        string message = "There are one or more invalid input values.";
        context.Result = new BadRequestObjectResult(new { code, message });
        context.ExceptionHandled = true;
    }
}
