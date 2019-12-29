using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

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
                    msg = "很抱歉，您无权访问该接口，请确保已经登录!";
                else if (statusCode == 403)
                    msg = "很抱歉，您的访问权限等级不够，联系管理员!";
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
        private static async Task HandleExceptionAsync(HttpContext context, int statusCode, string msg)
        {
            #region 解析Response
            //AnalysisResponse a = new AnalysisResponse();
            //    var dwy = a.ReadBodyAsync(context.Response);
            //    var result = dwy.Result;
            //    return Task.CompletedTask;
            #endregion

            var result = Newtonsoft.Json.JsonConvert.SerializeObject(new MessageModel() { Success = false, Message = msg, Code = statusCode.ToString() });
            context.Response.ContentType = "application/json;charset=utf-8";
            await context.Response.WriteAsync(result);
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
