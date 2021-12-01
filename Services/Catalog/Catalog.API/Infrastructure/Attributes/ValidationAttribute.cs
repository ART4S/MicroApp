using FluentValidation;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;

namespace Catalog.API.Infrastructure.Attributes;

public class ValidationAttribute : ActionFilterAttribute
{
    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.ModelState.IsValid)
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

            await context.HttpContext.Response.WriteAsJsonAsync(new
            {
                Errors = context.ModelState
                    .Where(x => x.Value is not null && x.Value.Errors.Count > 0)
                    .ToDictionary(x => x.Key, x => x.Value.Errors.Select(x => x.ErrorMessage))
            }, new JsonSerializerOptions() { WriteIndented = true });

            return;
        }

        await next();
    }
}
