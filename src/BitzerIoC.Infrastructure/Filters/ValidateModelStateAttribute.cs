using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BitzerIoC.Infrastructure.Filters
{
    /// <summary>
    /// ToDo: Not Implemeneted
    /// </summary>
    public class ValidateModelStateAttribute: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new ContentResult
                {
                    Content     =  $"Error: Model State  is :{context.ModelState}",
                    ContentType =  "text/plain",
                    StatusCode  =  (int?)HttpStatusCode.BadRequest
                };
            }
        }
    }
}


