using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using RiPOS.Shared.Models.Responses;

namespace RiPOS.API.Utilities.ActionFilters;

public class ModelValidationAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var firstError = context.ModelState.Values
                .SelectMany(v => v.Errors.Select(x => x.ErrorMessage))
                .FirstOrDefault();

            var response = new SimpleResponse(firstError ?? "Modelo inválido");

            context.Result = new BadRequestObjectResult(response);

        }
        base.OnActionExecuting(context);
    }
}