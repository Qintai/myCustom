using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace QinOpen.Middleware
{
    /// <summary>
    /// // https://www.cnblogs.com/jomzhang/p/9253647.html
    /// 
    /// </summary>
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                var statusCode = context.Response.StatusCode;
                if (ex is ArgumentException)
                {
                    statusCode = 200;
                }
                await HandleExceptionAsync(context, statusCode, ex.Message);
            }
            finally
            {
                var statusCode = context.Response.StatusCode;
                var msg = "";
                if (statusCode == 401)
                    msg = "未授权";
                else if (statusCode == 404)
                    msg = "未找到服务";
                else if (statusCode == 400)
                    msg = "验证错误";
                else if (statusCode == 502)
                    msg = "请求错误";
                else if (statusCode != 200)
                    msg = "未知错误";
                if (!string.IsNullOrWhiteSpace(msg))
                    await HandleExceptionAsync(context, statusCode, msg);
            }
        }
        //异常错误信息捕获，将错误信息用Json方式返回
        private static Task HandleExceptionAsync(HttpContext context, int statusCode, string msg)
        {
            context.Response.OnStarting(() => 
            {
                A a = new A();
                var dwy = a.ReadBodyAsync(context.Response);
                var result = dwy.Result;
                return Task.CompletedTask;
            });

            return Task.CompletedTask;
            //var result = Newtonsoft.Json.JsonConvert.SerializeObject(new MessageModel() { success = false, msg = msg, code = statusCode.ToString() });
            //context.Response.ContentType = "application/json;charset=utf-8";
            //await context.Response.WriteAsync(result);
        }
    }

    //扩展方法
    public static class ErrorHandlingExtensions
    {
        public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }

}
