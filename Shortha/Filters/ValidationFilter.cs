using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Shortha.Application.Exceptions;

namespace Shortha.Filters;

// This filter captures model state errors (including FluentValidation) and throws our custom ValidationException
public class ValidationFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => string.IsNullOrWhiteSpace(e.ErrorMessage) ? "Invalid value." : e.ErrorMessage)
                .Distinct()
                .ToList();

            throw new ValidationException(errors);
        }
    }

    public void OnActionExecuted(ActionExecutedContext context) { }
}
