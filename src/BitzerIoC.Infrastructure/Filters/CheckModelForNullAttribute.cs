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
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class CheckModelForNullAttribute:ActionFilterAttribute
    {
        //T is dictionary and returrn in bool and name is function _validate
        private readonly Func<Dictionary<string, object>, bool> _validate;

        public CheckModelForNullAttribute() : this(arguments =>arguments.ContainsValue(null))
        {
        }


        public CheckModelForNullAttribute(Func<Dictionary<string, object>, bool> checkCondition)
        {
            _validate = checkCondition;
        }

        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            if (_validate(actionContext.ActionArguments as Dictionary<string,object>))
            {
                actionContext.Result = new ContentResult
                {
                    Content = $"Error: Null argument",
                    ContentType = "text/plain",
                    StatusCode = (int?)HttpStatusCode.BadRequest
                };
            }
        }
    }
}
