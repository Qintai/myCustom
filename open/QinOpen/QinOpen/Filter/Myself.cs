using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
/*
HttpResponse也是一个抽象类，我们使用它来输出对请求的响应，如设置HTTP状态码，Cookies，HTTP响应报文头，响应主体等，
以及提供了一些将响应发送到客户端时的相关事件。其 HasStarted 属性用来表示响应是否已开始发往客户端，
在我们第一次调用 response.Body.WriteAsync 方法时，该属性便会被设置为 True。
需要注意的是，一旦 HasStarted 设置为 true 后，便不能再修改响应头，否则将会抛出 InvalidOperationException 异常，
也建议我们在HasStarted设置为true后，不要再对 Response 进行写入，因为此时 content-length 的值已经确定，
继续写入可能会造成协议冲突。HttpResponse 的默认实现为 DefaultHttpResponse ，它与 DefaultHttpRequest 类似，
只是对 IHttpResponseFeature 的封装，不过 ASP.NET Core 也为我们提供了一些扩展方法，如：我们在写入响应时，
通常使用的是 Response 的扩展方法 WriteAsync ：
————————————————
原文链接：https://blog.csdn.net/sD7O95O/article/details/78096047
*/
namespace QinOpen.Filter
{
    /// <summary>
    /// 资源Filter，可以拦截到
    /// </summary>
    public class CustomResource : Attribute, IResourceFilter
    {
        public void OnResourceExecuted(ResourceExecutedContext context)
        {

            #region 因为相应已经发生，流无法读取
            //string str;
            //using (StreamReader sr = new StreamReader(context.HttpContext.Response.Body, Encoding.UTF8, true, 1024, true))//这里注意Body部分不能随StreamReader一起释放
            //{
            //    str = sr.ReadToEnd();
            //}
            //context.HttpContext.Response.Clear();
            #endregion

            // 响应已经发起了，就别想着再改动了
            //Clear(context.HttpContext.Response);

            context.HttpContext.Response.WriteAsync("444444"); //这里这么些，会追加到 之前内容的后面
        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        { }

        #region HttpResponse的Clear源码
        /// <summary>
        /// 这个方法就是   context.HttpContext.Response.Clear()
        /// response.HasStarted=true。代表请求，已经发起，无法撤销
        /// </summary>
        /// <param name="response"></param>
        public void Clear(HttpResponse response)
        {
            if (response.HasStarted)
                // 无法清除响应，它已开始发送。
                throw new InvalidOperationException("The response cannot be cleared, it has already started sending.");

            response.StatusCode = 200;
            response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = null;
            response.Headers.Clear();
            if (response.Body.CanSeek)
                response.Body.SetLength(0);
        }
        #endregion

    }


    /// <summary>
    /// Startup 加入 全局，Startup 还需配置 services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);  禁用自带 的验证
    /// </summary>
    public class GlobalAction : IActionFilter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuted(ActionExecutedContext context)
        { }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
                context.Result = new ValidationFailedResult(context.ModelState);
        }

        /// <summary>
        ///  局部。Startup 还需配置 services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true); //禁用自带 的验证
        /// </summary>
        public class CustomActionAttribute : Attribute, IActionFilter
        {

            public void OnActionExecuted(ActionExecutedContext context)
            { }

            public void OnActionExecuting(ActionExecutingContext context)
            {
                if (!context.ModelState.IsValid)
                    context.Result = new ValidationFailedResult(context.ModelState);
            }
        }
    }
}



