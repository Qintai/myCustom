using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace crud.Web.Custom.Filter
{
    public class ActionControllerActionFilterAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            var callbackValue=  context.HttpContext.Request.Query?["callback"].ToString();
            if (!string.IsNullOrWhiteSpace(callbackValue))
            {
                if (context.RouteData.Values.Count > 0)
                {
                    var value = ((ObjectResult)context.Result).Value;
                    value = $"{callbackValue}({value})";
                    ObjectResult objectResult = new ObjectResult(value);
                    context.Result = objectResult;
                }
            }
         }

        public void OnActionExecuting(ActionExecutingContext context)
        {}
    }
}
