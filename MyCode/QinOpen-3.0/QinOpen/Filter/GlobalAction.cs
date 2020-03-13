using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;

namespace QinOpen.Filter
{
    /// <summary>
    /// Startup 加入 全局，Startup 还需配置 禁用自带 的验证
    /// </summary>
    public class GlobalAction : IActionFilter
    {
        // services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true)

        public void OnActionExecuted(ActionExecutedContext context)
        {
            #region 是否返回JsonP
            var callbackValue = context.HttpContext.Request.Query?["callback"].ToString();
            if (!string.IsNullOrWhiteSpace(callbackValue))
            {
                if (context.RouteData.Values.Count > 0 && context.Result != null)
                {
                    var value = ((ObjectResult)context.Result).Value;
                    value = $"{callbackValue}({value})";
                    ObjectResult objectResult = new ObjectResult(value);
                    context.Result = objectResult;
                    //context.HttpContext.Response.ContentType = "application/javascript; charset=utf-8";
                    //context.HttpContext.Response.WriteAsync(value.ToString());
                }
            }
            #endregion
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
                context.Result = new ValidationFailedResult(context.ModelState);
        }

    }
}



