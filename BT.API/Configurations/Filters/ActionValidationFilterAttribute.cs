using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BT.API.Configurations.Filters
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ActionValidationFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(context.ModelState);
            }

            if (context.HttpContext != null && context.HttpContext.User != null)
            {
                var identity = new ClaimsIdentity(context.HttpContext.User.Identity);
                var principal = new ClaimsPrincipal(identity);
                Thread.CurrentPrincipal = principal;
            }
        }
    }
}