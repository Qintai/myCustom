﻿using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace QinOpen.Filter
{
    /// <summary>
    ///  局部ActionFilter。 Startup 还需配置 禁用自带 的验证
    /// </summary>
    public class CustomActionAttribute : Attribute, IActionFilter
    {
        //services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true)

        public void OnActionExecuted(ActionExecutedContext context)
        { }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
                context.Result = new ValidationFailedResult(context.ModelState);
        }
    }
}
