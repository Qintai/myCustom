using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using QinCommon;
using System;
namespace QinOpen.Filter
{
    /// <summary>
    /// 全局异常错误日志
    /// </summary>
    public class GlobalExceptionsFilter : IExceptionFilter
    {
        //private readonly IWebHostEnvironment _env;
        //private readonly ILogger<GlobalExceptionsFilter> _loggerHelper;
        private static readonly log4net.ILog log
        = log4net.LogManager.GetLogger(typeof(GlobalExceptionsFilter));


        public GlobalExceptionsFilter(IWebHostEnvironment env, ILogger<GlobalExceptionsFilter> loggerHelper)
        {
            //_env = env;
            //_loggerHelper = loggerHelper;
        }

        public void OnException(ExceptionContext context)
        {
            MessageModel messageModel = new MessageModel
            {
                Msg = "出错了"
            };
            context.Result = new JsonResult(messageModel);
            //采用log4net 进行错误日志记录

            //   Log4helper<GlobalExceptionsFilter>.Errror(context.Exception);

            log.Error(context.Exception.Message + WriteLog("程序出错", context.Exception));
        }

        /// <summary>
        /// 自定义返回格式
        /// </summary>
        /// <param name="throwMsg"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        public string WriteLog(string throwMsg, Exception ex)
        {
            var obj = new object[] { throwMsg, ex.GetType().Name, ex.Message, ex.StackTrace };
            return string.Format("\r\n【自定义错误】：{0} \r\n【异常类型】：{1} \r\n【异常信息】：{2} \r\n【堆栈调用】：{3}",
               obj);
        }

    }
}
