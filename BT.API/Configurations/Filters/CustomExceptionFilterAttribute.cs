using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BT.API.Configurations.Filters
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {

        /// <summary>
        /// Handle Exceptions here.abstract By Default it will return a 500 error
        /// </summary>
        /// <param name="context"></param>
        public override void OnException(ExceptionContext context)
        {
            var code = HttpStatusCode.InternalServerError;
            // if (context.Exception is RecordNotFoundException)
            // {
            //     code = HttpStatusCode.NotFound;
            // }

            context.HttpContext.Response.ContentType = "application/json";
            context.HttpContext.Response.StatusCode = (int)code;
            context.Result = new JsonResult(new
            {
                error = context.Exception.Message,
                stackTrace = context.Exception.StackTrace
            });
        }
    }
}